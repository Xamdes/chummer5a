using Chummer;
using ChummerHub.Client.UI;
using SINners.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChummerHub.Client.Backend
{


    public class SearchTagExtractor
    {



        /// <summary>
        /// This function searches recursivly through the Object "obj" and generates Tags for each
        /// property found with an HubTag-Attribute.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parenttag"></param>
        /// <returns>A list of Tags (that may have a lot of child-Tags as well).</returns>
        internal static IList<SearchTag> ExtractTagsFromAttributes(object obj, SearchTag parenttag)
        {
            List<SearchTag> resulttags = new List<SearchTag>();
            List<Tuple<PropertyInfo, Chummer.HubTagAttribute, object>> props = new List<Tuple<PropertyInfo, Chummer.HubTagAttribute, object>>();


            List<PropertyInfo> test = (from p in obj.GetType().GetProperties() select p).ToList();
            List<object[]> test2 = (from p in obj.GetType().GetProperties() let attr = p.GetCustomAttributes(typeof(Chummer.HubTagAttribute), true) select attr).ToList();
            Type[] test4 = obj.GetType().GenericTypeArguments;



            IEnumerable<Tuple<PropertyInfo, HubTagAttribute, object>> props2 = from p in obj.GetType().GetProperties()
                                                                               let attr = p.GetCustomAttributes(typeof(Chummer.HubTagAttribute), true)
                                                                               where attr.Length > 0
                                                                               select new Tuple<PropertyInfo, Chummer.HubTagAttribute, object>(p, attr.First() as Chummer.HubTagAttribute, obj);

            props.AddRange(props2);


            if (!typeof(string).IsAssignableFrom(obj.GetType()))
            {
                IEnumerable islist = obj as IEnumerable;

                if (islist == null)
                {
                    islist = obj as ICollection;
                }
                if (islist != null)
                {
                    Type listtype = StaticUtils.GetListType(islist);
                    try
                    {
                        object generic = Activator.CreateInstance(listtype, new object[] { ucSINnersSearch.MySearchCharacter.MyCharacter });
                        IList<SearchTag> result = ExtractTagsFromAttributes(generic, parenttag);
                        return result;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            object generic = Activator.CreateInstance(listtype);
                            IList<SearchTag> result = ExtractTagsFromAttributes(generic, parenttag);
                            return result;
                        }
                        catch (Exception e2)
                        {
                            //seriously, that gets out of hand...
                            System.Diagnostics.Trace.TraceError(e2.ToString());
                            throw;
                        }
                    }

                }
            }

            foreach (Tuple<PropertyInfo, HubTagAttribute, object> prop in props)
            {
                SearchTag temptag = new SearchTag(prop.Item1, prop.Item2)
                {
                    MyParentTag = parenttag,
                    SearchOpterator = EnumSSearchOpterator.bigger.ToString(),
                    TagName = prop.Item1.Name,
                    TagValue = "",
                    MyRuntimePropertyValue = prop.Item1.GetValue(prop.Item3)
                };
                resulttags.Add(temptag);
            }
            return resulttags;
        }


        internal static IList<SearchTag> ExtractTagsFromAttributesForProperty(Tuple<PropertyInfo, Chummer.HubTagAttribute, object> prop, SearchTag parenttag)
        {
            List<SearchTag> proptaglist = new List<SearchTag>();
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


            SearchTag tag = new SearchTag(property, attribute)
            {
                MyRuntimePropertyValue = propValue,
                SearchTags = new List<SearchTag>(),
                MyParentTag = parenttag
            };
            if (tag.MyParentTag != null)
            {
                tag.MyParentTag.SearchTags.Add(tag);
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
                object addObject = t.GetProperty(attribute.TagNameFromProperty).GetValue(prop.Item3, null);
                tag.TagName += string.Format("{0}", addObject);
            }
            tag.TagValue = string.Format("{0}", tag.MyRuntimePropertyValue);
            Type typeValue = tag.MyRuntimePropertyValue.GetType();
            //if (typeof(int).IsAssignableFrom(typeValue))
            //{
            //    tag.TagType = "int";
            //}
            //else if (typeof(double).IsAssignableFrom(typeValue))
            //{
            //    tag.TagType = "double";
            //}
            //else if (typeof(bool).IsAssignableFrom(typeValue))
            //{
            //    tag.TagType = "bool";
            //}
            //else if (typeof(string).IsAssignableFrom(typeValue))
            //{
            //    tag.TagType = "string";
            //}
            //else if (typeof(Guid).IsAssignableFrom(typeValue))
            //{
            //    tag.TagType = "Guid";
            //}
            //else
            //{
            //    tag.TagType = "other";
            //}
            //if (tag.TagValue == typeValue.FullName)
            //    tag.TagValue = "";
            //if ((typeof(IEnumerable).IsAssignableFrom(typeValue)
            //    || typeof(ICollection).IsAssignableFrom(typeValue))
            //    && !typeof(string).IsAssignableFrom(typeValue))
            //{
            //    tag.TagType = "list";
            //    tag.TagValue = "";
            //}
            //if (!String.IsNullOrEmpty(attribute.TagValueFromProperty))
            //{
            //    var addObject = t.GetProperty(attribute.TagValueFromProperty).GetValue(prop.Item3, null);
            //    tag.TagValue = String.Format("{0}", addObject);
            //}
            proptaglist.Add(tag);
            if (prop.Item1 != null)
            {
                IList<SearchTag> childlist = ExtractTagsFromAttributes(tag.MyRuntimePropertyValue, tag);
                proptaglist.AddRange(childlist);
            }
            if (attribute.DeleteIfEmpty)
            {
                if (!tag.Tags.Any() && string.IsNullOrEmpty(tag.TagValue))
                {
                    tag.MyParentTag.SearchTags.Remove(tag);
                }
            }
            return proptaglist;
        }
    }
}
