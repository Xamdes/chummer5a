/*  This file is part of Chummer5a.
 *
 *  Chummer5a is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Chummer5a is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Chummer5a.  If not, see <http://www.gnu.org/licenses/>.
 *
 *  You can obtain the full source code for Chummer5a at
 *  https://github.com/chummer5a/chummer5a
 */
using System;
using System.Collections.Generic;

namespace Chummer
{
    /// <summary>
    /// How should instances of this Class be tagged?
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HubClassTagAttribute : System.Attribute
    {
        //private string _ListName;
        private readonly string _ListInstanceNameFromProperty;
        private readonly bool _DeleteEmptyTags = false;
        private readonly List<string> _CommentProperties = new List<string>();
        private readonly List<string> _ExtraProperties = new List<string>();


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="listInstanceNameFromProperty"></param>
        /// <param name="deleteEmptyTags"></param>
        /// <param name="commentProperties">a list of Properties to tag - delimiter is ";"</param>
        /// <param name="extraProperties"></param>
        public HubClassTagAttribute(string listInstanceNameFromProperty, bool deleteEmptyTags, string commentProperties, string extraProperties)
        {
            //_ListName = ListName;
            _ListInstanceNameFromProperty = listInstanceNameFromProperty;
            _DeleteEmptyTags = deleteEmptyTags;
            if (!string.IsNullOrEmpty(commentProperties))
            {
                _CommentProperties = new List<string>(commentProperties.Split(';'));
            }

            if (!string.IsNullOrEmpty(extraProperties))
            {
                _ExtraProperties = new List<string>(extraProperties.Split(';'));
            }
        }

        public List<string> ListCommentProperties => _CommentProperties;

        public List<string> ListExtraProperties => _ExtraProperties;


        public string ListInstanceNameFromProperty => _ListInstanceNameFromProperty;
        public bool DeleteEmptyTags => _DeleteEmptyTags;

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class HubTagAttribute : System.Attribute
    {
        private readonly string _TagName;
        private readonly string _TagValueFromProperty;
        private readonly string _TagNameFromProperty;
        private readonly bool _deleteIfEmpty = false;

        public HubTagAttribute()
        {

        }

        public HubTagAttribute(
                  string TagName,
                  string TagNameFromProperty,
                  string TagValueFromProperty,
                  bool deleteIfEmpty)
        {
            _TagName = TagName;
            _TagNameFromProperty = TagNameFromProperty;
            _TagValueFromProperty = TagValueFromProperty;
            _deleteIfEmpty = deleteIfEmpty;
        }


        public HubTagAttribute(
          string TagName,
          string TagNameFromProperty)
        {
            _TagName = TagName;
            _TagNameFromProperty = TagNameFromProperty;
        }

        public HubTagAttribute(
          string TagName) => _TagName = TagName;

        public HubTagAttribute(
          bool deleteIfEmpty) => _deleteIfEmpty = deleteIfEmpty;

        public string TagName => _TagName;

        public string TagNameFromProperty => _TagNameFromProperty;

        public string TagValueFromProperty => _TagValueFromProperty;

        public bool DeleteIfEmpty => _deleteIfEmpty;




    }
}
