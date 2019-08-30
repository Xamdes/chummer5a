using Chummer;
using SINners.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ChummerHub.Client.Backend
{
    public static class TagExtractor
    {
        private static IEnumerable<Type> _AllChummerTypes = null;
        public static IEnumerable<Type> AllChummerTypes
        {
            get
            {
                if (_AllChummerTypes == null)
                {
                    _AllChummerTypes = typeof(Character).Assembly.GetTypes();
                }
                return _AllChummerTypes;
            }
        }

        public static Dictionary<int, object> MyReflectionCollection
        {
            get; set;
        }

        /// <summary>
        /// This function searches recursivly through the Object "obj" and generates Tags for each
        /// property found with an HubTag-Attribute.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parenttag"></param>
        /// <returns>A list of Tags (that may have a lot of child-Tags as well).</returns>
        internal static IList<Tag> ExtractTagsFromAttributes(object obj, Tag parenttag)
        {
            List<Tag> resulttags = new List<Tag>();
            List<Tuple<PropertyInfo, Chummer.HubTagAttribute, object>> props = new List<Tuple<PropertyInfo, Chummer.HubTagAttribute, object>>();


            IEnumerable<Tuple<PropertyInfo, HubTagAttribute, object>> props2 = from p in obj.GetType().GetProperties()
                                                                               let attr = p.GetCustomAttributes(typeof(Chummer.HubTagAttribute), true)
                                                                               where attr.Length == 1
                                                                               select new Tuple<PropertyInfo, Chummer.HubTagAttribute, object>(p, attr.First() as Chummer.HubTagAttribute, obj);

            props.AddRange(props2);

            if (!typeof(string).IsAssignableFrom(obj.GetType()))
            {
                if (obj.GetType().Assembly.FullName.Contains("Chummer"))
                {
                    //check, if the type has an HubClassTagAttribute
                    List<Tuple<Chummer.HubClassTagAttribute, object>> classprops = (from p in obj.GetType().GetCustomAttributes(typeof(Chummer.HubClassTagAttribute), true)
                                                                                    select new Tuple<Chummer.HubClassTagAttribute, object>(p as Chummer.HubClassTagAttribute, obj)).ToList();
                    if (classprops.Any())
                    {
                        foreach (Tuple<HubClassTagAttribute, object> classprop in classprops)
                        {
                            Tag tag = new Tag(obj, classprop.Item1)
                            {
                                SiNnerId = parenttag.SiNnerId,
                                ParentTagId = parenttag.Id,
                                MyParentTag = parenttag
                            };
                            parenttag.Tags.Add(tag);
                            resulttags.Add(tag);
                            tag.MyRuntimeHubClassTag = classprop.Item1;
                            //tag.TagName = classprop.Item1.ListName;
                            SetTagTypeEnumFromCLRType(tag, obj.GetType());
                            if (!string.IsNullOrEmpty(classprop.Item1.ListInstanceNameFromProperty))
                            {
                                tag.TagName = classprop.Item1.ListInstanceNameFromProperty;
                                IEnumerable<PropertyInfo> childprop = from p in obj.GetType().GetProperties()
                                                                      where p.Name == classprop.Item1.ListInstanceNameFromProperty
                                                                      select p;
                                if (!childprop.Any())
                                {
                                    throw new ArgumentOutOfRangeException("Could not find property " + classprop.Item1.ListInstanceNameFromProperty + " on instance of type " + obj.GetType().ToString() + ".");
                                }

                                tag.TagValue += childprop.FirstOrDefault().GetValue(obj);
                                if (double.TryParse(tag.TagValue, out double outdouble))
                                {
                                    tag.TagValueDouble = outdouble;
                                }
                            }
                            if (string.IsNullOrEmpty(tag.TagName))
                            {
                                tag.TagName = obj.ToString();
                            }

                            ExtractTagsAddIncludeProperties(obj, resulttags, classprop, tag);
                        }
                        return resulttags;
                    }

                }
                IEnumerable islist = obj as IEnumerable;

                if (islist == null)
                {
                    islist = obj as ICollection;
                }
                if (islist != null)
                {
                    int counter = 0;
                    foreach (object item in islist)
                    {
                        counter++;
                        List<Tuple<Chummer.HubClassTagAttribute, object>> classprops = (from p in item.GetType().GetCustomAttributes(typeof(Chummer.HubClassTagAttribute), true)
                                                                                        select new Tuple<Chummer.HubClassTagAttribute, object>(p as Chummer.HubClassTagAttribute, obj)).ToList();
                        foreach (Tuple<HubClassTagAttribute, object> classprop in classprops)
                        {
                            Tag tag = new Tag(item, classprop.Item1)
                            {
                                SiNnerId = parenttag.SiNnerId,
                                ParentTagId = parenttag.Id,
                                MyParentTag = parenttag
                            };
                            parenttag.Tags.Add(tag);
                            resulttags.Add(tag);
                            tag.MyRuntimeHubClassTag = classprop.Item1;
                            //tag.TagName = classprop.Item1.ListName;
                            tag.TagType = "list";
                            if (!string.IsNullOrEmpty(classprop.Item1.ListInstanceNameFromProperty))
                            {
                                tag.TagName = classprop.Item1.ListInstanceNameFromProperty;
                                IEnumerable<PropertyInfo> childprop = from p in item.GetType().GetProperties()
                                                                      where p.Name == classprop.Item1.ListInstanceNameFromProperty
                                                                      select p;
                                if (!childprop.Any())
                                {
                                    throw new ArgumentOutOfRangeException("Could not find property " + classprop.Item1.ListInstanceNameFromProperty + " on instance of type " + item.GetType().ToString() + ".");
                                }

                                tag.TagValue += childprop.FirstOrDefault().GetValue(item);
                                if (double.TryParse(tag.TagValue, out double outdouble))
                                {
                                    tag.TagValueDouble = outdouble;
                                }
                            }
                            if (string.IsNullOrEmpty(tag.TagName))
                            {
                                tag.TagName = item.ToString();
                            }

                            ExtractTagsAddIncludeProperties(item, resulttags, classprop, tag);
                        }
                    }
                    if (counter == 0)
                    {
                        //this whole tree is empty - remove it!
                        parenttag.MyParentTag.Tags.Remove(parenttag);
                        return null;
                    }
                    return resulttags;
                }
            }

            foreach (Tuple<PropertyInfo, HubTagAttribute, object> prop in props)
            {
                IList<Tag> proptags = ExtractTagsFromAttributesForProperty(prop, parenttag);
                resulttags.AddRange(proptags);
            }
            return resulttags;
        }

        private static void ExtractTagsAddIncludeProperties(object obj, List<Tag> resulttags, Tuple<Chummer.HubClassTagAttribute, object> classprop, Tag tag)
        {
            //add the TagComment
            foreach (string includeprop in classprop.Item1.ListCommentProperties)
            {
                IEnumerable<PropertyInfo> propfoundseq = from p in obj.GetType().GetProperties()
                                                         where p.Name == includeprop
                                                         select p;
                if (!propfoundseq.Any())
                {
                    throw new ArgumentOutOfRangeException("Could not find property " + includeprop + " on instance of type " + obj.GetType().ToString() + ".");
                }
                object includeInstance = propfoundseq.FirstOrDefault().GetValue(obj);
                if (includeInstance != null)
                {
                    tag.TagComment += includeInstance.ToString() + " ";
                }
            }
            tag.TagComment = tag.TagComment.TrimEnd(" ");
            //add the "Extra" to this Instance
            foreach (string includeprop in classprop.Item1.ListExtraProperties)
            {
                IEnumerable<PropertyInfo> propfoundseq = from p in obj.GetType().GetProperties()
                                                         where p.Name == includeprop
                                                         select p;
                if (!propfoundseq.Any())
                {
                    throw new ArgumentOutOfRangeException("Could not find property " + includeprop + " on instance of type " + obj.GetType().ToString() + ".");
                }
                object includeInstance = propfoundseq.FirstOrDefault().GetValue(obj);
                if (includeInstance != null && !string.IsNullOrEmpty(includeInstance.ToString()))
                {
                    Tag instanceTag = new Tag(includeInstance, classprop.Item1)
                    {
                        SiNnerId = tag.SiNnerId,
                        ParentTagId = tag.Id,
                        MyParentTag = tag
                    };
                    tag.Tags.Add(instanceTag);
                    resulttags.Add(instanceTag);
                    instanceTag.MyRuntimeHubClassTag = classprop.Item1;
                    instanceTag.TagName = includeprop;
                    SetTagTypeEnumFromCLRType(instanceTag, obj.GetType());
                    instanceTag.TagValue = includeInstance.ToString();
                    if (double.TryParse(tag.TagValue, out double outdouble))
                    {
                        tag.TagValueDouble = outdouble;
                    }
                }
            }

        }

        internal static IList<Tag> ExtractTagsFromAttributesForProperty(Tuple<PropertyInfo, Chummer.HubTagAttribute, object> prop, Tag parenttag)
        {
            List<Tag> proptaglist = new List<Tag>();
            Chummer.HubTagAttribute attribute = prop.Item2 as Chummer.HubTagAttribute;
            object propValue;
            PropertyInfo property = prop.Item1 as PropertyInfo;
            if (property == null)
            {
                propValue = prop.Item3;
            }
            else
            {
                propValue = property.GetValue(prop.Item3);
                if (propValue == null)
                {
                    //dont save "null" values
                    return proptaglist;
                }
                if (propValue.GetType().IsAssignableFrom(typeof(bool)))
                {
                    if (propValue as bool? == false)
                    { //dont save "false" values
                        return proptaglist;
                    }
                }
                if (propValue.GetType().IsAssignableFrom(typeof(int)))
                {
                    if (propValue as int? == 0)
                    {   //dont save "0" values
                        return proptaglist;
                    }
                }
            }


            Tag tag = new Tag(propValue, attribute)
            {
                SiNnerId = parenttag?.SiNnerId,
                Tags = new List<Tag>(),
                MyParentTag = parenttag
            };
            if (tag.MyParentTag != null)
            {
                tag.MyParentTag.Tags.Add(tag);
            }

            tag.ParentTagId = parenttag?.Id;
            tag.Id = Guid.NewGuid();
            if (!string.IsNullOrEmpty(attribute.TagName))
            {
                tag.TagName = attribute.TagName;
            }
            else if (prop.Item1 != null)
            {
                tag.TagName = prop.Item1.Name;
            }
            else
            {
                tag.TagName = prop.Item3.ToString();
            }

            Type t = prop.Item3.GetType();
            if (!string.IsNullOrEmpty(attribute.TagNameFromProperty))
            {
                try
                {
                    object addObject = t.GetProperty(attribute.TagNameFromProperty).GetValue(prop.Item3, null);
                    tag.TagName += string.Format("{0}", addObject);
                }
                catch (Exception)
                {
#if DEBUG
                    Debugger.Break();
#else
                    throw;
#endif
                }

            }
            tag.TagValue = string.Format("{0}", tag.MyRuntimeObject);
            if (double.TryParse(tag.TagValue, out double outdouble1))
            {
                tag.TagValueDouble = outdouble1;
            }

            Type typeValue = tag.MyRuntimeObject.GetType();
            SetTagTypeEnumFromCLRType(tag, typeValue);
            if (!string.IsNullOrEmpty(attribute.TagValueFromProperty))
            {
                object addObject = t.GetProperty(attribute.TagValueFromProperty).GetValue(prop.Item3, null);
                tag.TagValue = string.Format("{0}", addObject);
                if (double.TryParse(tag.TagValue, out double outdouble2))
                {
                    tag.TagValueDouble = outdouble2;
                }
            }
            proptaglist.Add(tag);
            if (prop.Item1 != null)
            {
                IList<Tag> childlist = ExtractTagsFromAttributes(tag.MyRuntimeObject, tag);
                if (childlist != null)
                {
                    proptaglist.AddRange(childlist);
                }
            }
            if (attribute.DeleteIfEmpty)
            {
                if (!tag.Tags.Any() && string.IsNullOrEmpty(tag.TagValue))
                {
                    tag.MyParentTag.Tags.Remove(tag);
                }
            }
            return proptaglist;
        }

        private static void SetTagTypeEnumFromCLRType(Tag tag, Type typeValue)
        {
            if (typeof(int).IsAssignableFrom(typeValue))
            {
                tag.TagType = "int";
            }
            else if (typeof(double).IsAssignableFrom(typeValue))
            {
                tag.TagType = "double";
            }
            else if (typeof(bool).IsAssignableFrom(typeValue))
            {
                tag.TagType = "bool";
            }
            else if (typeof(string).IsAssignableFrom(typeValue))
            {
                tag.TagType = "string";
            }
            else if (typeof(Guid).IsAssignableFrom(typeValue))
            {
                tag.TagType = "Guid";
            }
            else
            {

                tag.TagType = "other";
            }

            if (tag.TagValue == typeValue.FullName)
            {
                tag.TagValue = "";
                tag.TagValueDouble = null;
            }

            if ((typeof(IEnumerable).IsAssignableFrom(typeValue)
                || typeof(ICollection).IsAssignableFrom(typeValue))
                && !typeof(string).IsAssignableFrom(typeValue))
            {
                tag.TagType = "list";
                tag.TagValue = "";
                tag.TagValueDouble = null;
            }
        }

        internal static IList<Tag> ExtractTags(object obj, int level, Tag parenttag)
        {
            if (MyReflectionCollection == null)
            {
                MyReflectionCollection = new Dictionary<int, object>();
            }

            List<Tag> resulttags = new List<Tag>();
            if (obj == null)
            {
                return resulttags;
            }

            if (MyReflectionCollection.ContainsKey(obj.GetHashCode()))
            {
                return resulttags;
            }

            MyReflectionCollection.Add(obj.GetHashCode(), obj);

            Type type = obj.GetType();
            if (!AllChummerTypes.Contains(type))
            {
                return resulttags;
            }

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                object propValue = null;
                try
                {
                    propValue = property.GetValue(obj, null);
                }
                catch (Exception)
                {
                    System.Diagnostics.Debugger.Break();
                    continue;
                }
                if (propValue == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(propValue.ToString()))
                {
                    continue;
                }

                IList elems = propValue as IList;
                if (elems != null)
                {
                    foreach (object item in elems)
                    {
                        IList<Tag> itemtags = ExtractTags(item, level - 1, parenttag);
                        if (parenttag == null)
                        {
                            resulttags.AddRange(itemtags);
                        }
                    }
                }
                else
                {

                    //check if the propValue is a parent
                    Tag tempParent = parenttag;
                    bool found = false;
                    while (tempParent != null)
                    {
                        if (tempParent.MyRuntimeObject == propValue)
                        {
                            found = true;
                            break;
                        }
                        tempParent = tempParent.MyParentTag;
                    }
                    if (found)
                    {
                        continue;
                    }

                    object propValue1 = property.GetValue(obj);
                    Tag tag = new Tag
                    {
                        Tags = new List<Tag>(),
                        MyParentTag = parenttag
                    };
                    if (tag.MyParentTag != null)
                    {
                        tag.MyParentTag.Tags.Add(tag);
                    }

                    tag.ParentTagId = parenttag?.Id;
                    tag.Id = Guid.NewGuid();
                    tag.TagName = property.Name;

                    tag.MyRuntimeObject = propValue1;
                    tag.TagValue = string.Format("{0}", propValue1);
                    if (double.TryParse(tag.TagValue, out double outdouble))
                    {
                        tag.TagValueDouble = outdouble;
                    }

                    if (level > 0)
                    {
                        IList<Tag> childtags = ExtractTags(propValue, level - 1, tag);
                    }
                    if ((tag.Tags.Count() == 0) && (string.IsNullOrEmpty(tag.TagValue)))
                    {
                        tag.MyParentTag.Tags.Remove(tag);
                    }
                    if (parenttag == null)
                    {
                        resulttags.Add(tag);
                    }
                }
            }
            return resulttags;
        }
    }
}
