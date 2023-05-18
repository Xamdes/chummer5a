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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Chummer.Annotations;
using NLog;

// ReSharper disable ConvertToAutoProperty

namespace Chummer.Backend.Equipment
{
    /// <summary>
    /// Type of Lifestyle.
    /// </summary>
    public enum LifestyleIncrement
    {
        Month = 0,
        Week = 1,
        Day = 2
    }

    /// <summary>
    /// Lifestyle.
    /// </summary>
    [DebuggerDisplay("{DisplayName(GlobalSettings.DefaultLanguage)}")]
    public sealed class Lifestyle : IHasInternalId, IHasXmlDataNode, IHasNotes, ICanRemove, IHasCustomName, IHasSourceId, IHasSource, ICanSort, INotifyMultiplePropertyChanged, IHasLockObject, IHasCost
    {
        private static readonly Lazy<Logger> s_ObjLogger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private static Logger Log => s_ObjLogger.Value;

        // ReSharper disable once InconsistentNaming
        private Guid _guiID;

        // ReSharper disable once InconsistentNaming
        private Guid _guiSourceID;

        private string _strName = string.Empty;
        private decimal _decCost;
        private int _intDice;
        private decimal _decMultiplier;
        private int _intIncrements = 1;
        private int _intRoommates;
        private decimal _decPercentage = 100.0m;
        private int _intComforts;
        private int _intArea;
        private int _intSecurity;
        private int _intBaseComforts;
        private int _intBaseArea;
        private int _intBaseSecurity;
        private int _intComfortsMaximum;
        private int _intSecurityMaximum;
        private int _intAreaMaximum;
        private int _intBonusLP;
        private int _intLP;
        private bool _blnAllowBonusLP;
        private bool _blnIsPrimaryTenant;
        private decimal _decCostForSecurity;
        private decimal _decCostForArea;
        private decimal _decCostForComforts;
        private string _strBaseLifestyle = string.Empty;
        private string _strSource = string.Empty;
        private string _strPage = string.Empty;
        private bool _blnTrustFund;
        private LifestyleType _eType = LifestyleType.Standard;
        private LifestyleIncrement _eIncrement = LifestyleIncrement.Month;
        private string _strNotes = string.Empty;
        private Color _colNotes = ColorManager.HasNotesColor;
        private int _intSortOrder;
        private readonly Character _objCharacter;

        private string _strCity;
        private string _strDistrict;
        private string _strBorough;

        #region Helper Methods

        /// <summary>
        /// Convert a string to a LifestyleType.
        /// </summary>
        /// <param name="strValue">String value to convert.</param>
        public static LifestyleType ConvertToLifestyleType(string strValue)
        {
            switch (strValue)
            {
                case "BoltHole":
                    return LifestyleType.BoltHole;

                case "Safehouse":
                    return LifestyleType.Safehouse;

                case "Advanced":
                    return LifestyleType.Advanced;

                default:
                    return LifestyleType.Standard;
            }
        }

        /// <summary>
        /// Convert a string to a LifestyleType.
        /// </summary>
        /// <param name="strValue">String value to convert.</param>
        public static LifestyleIncrement ConvertToLifestyleIncrement(string strValue)
        {
            switch (strValue)
            {
                case "day":
                case "Day":
                    return LifestyleIncrement.Day;

                case "week":
                case "Week":
                    return LifestyleIncrement.Week;

                default:
                    return LifestyleIncrement.Month;
            }
        }

        #endregion Helper Methods

        #region Constructor, Create, Save, Load, and Print Methods

        public Lifestyle(Character objCharacter)
        {
            // Create the GUID for the new Lifestyle.
            _guiID = Guid.NewGuid();
            _objCharacter = objCharacter;
            LifestyleQualities.CollectionChanged += LifestyleQualitiesCollectionChanged;
            LifestyleQualities.BeforeClearCollectionChanged += LifestyleQualitiesOnBeforeClearCollectionChanged;
        }

        /// <summary>
        /// Create a Lifestyle from an XmlNode and return the TreeNodes for it.
        /// </summary>
        /// <param name="objXmlLifestyle">XmlNode to create the object from.</param>
        public void Create(XmlNode objXmlLifestyle)
        {
            using (LockObject.EnterWriteLock())
            {
                if (!objXmlLifestyle.TryGetField("id", Guid.TryParse, out _guiSourceID))
                {
                    Log.Warn(new object[] {"Missing id field for xmlnode", objXmlLifestyle});
                    Utils.BreakIfDebug();
                }
                else
                {
                    _objCachedMyXmlNode = null;
                    _objCachedMyXPathNode = null;
                }

                objXmlLifestyle.TryGetStringFieldQuickly("name", ref _strBaseLifestyle);
                objXmlLifestyle.TryGetDecFieldQuickly("cost", ref _decCost);
                objXmlLifestyle.TryGetInt32FieldQuickly("dice", ref _intDice);
                objXmlLifestyle.TryGetDecFieldQuickly("multiplier", ref _decMultiplier);
                objXmlLifestyle.TryGetStringFieldQuickly("source", ref _strSource);
                objXmlLifestyle.TryGetStringFieldQuickly("page", ref _strPage);
                objXmlLifestyle.TryGetInt32FieldQuickly("lp", ref _intLP);
                objXmlLifestyle.TryGetDecFieldQuickly("costforarea", ref _decCostForArea);
                objXmlLifestyle.TryGetDecFieldQuickly("costforcomforts", ref _decCostForComforts);
                objXmlLifestyle.TryGetDecFieldQuickly("costforsecurity", ref _decCostForSecurity);
                objXmlLifestyle.TryGetBoolFieldQuickly("allowbonuslp", ref _blnAllowBonusLP);
                if (!objXmlLifestyle.TryGetMultiLineStringFieldQuickly("altnotes", ref _strNotes))
                    objXmlLifestyle.TryGetMultiLineStringFieldQuickly("notes", ref _strNotes);

                if (GlobalSettings.InsertPdfNotesIfAvailable && string.IsNullOrEmpty(Notes))
                {
                    Notes = CommonFunctions.GetBookNotes(objXmlLifestyle, Name, CurrentDisplayName, Source, Page,
                                                         DisplayPage(GlobalSettings.Language), _objCharacter);
                }

                string sNotesColor = ColorTranslator.ToHtml(ColorManager.HasNotesColor);
                objXmlLifestyle.TryGetStringFieldQuickly("notesColor", ref sNotesColor);
                _colNotes = ColorTranslator.FromHtml(sNotesColor);

                string strTemp = string.Empty;
                if (objXmlLifestyle.TryGetStringFieldQuickly("increment", ref strTemp))
                    _eIncrement = ConvertToLifestyleIncrement(strTemp);

                XPathNavigator xmlLifestyleXPathDocument = _objCharacter.LoadDataXPath("lifestyles.xml");
                XPathNavigator xmlLifestyleNode =
                    xmlLifestyleXPathDocument.SelectSingleNode(
                        "/chummer/comforts/comfort[name = " + BaseLifestyle.CleanXPath() + ']');
                xmlLifestyleNode.TryGetInt32FieldQuickly("minimum", ref _intBaseComforts);
                xmlLifestyleNode.TryGetInt32FieldQuickly("limit", ref _intComfortsMaximum);

                // Area.
                xmlLifestyleNode =
                    xmlLifestyleXPathDocument.SelectSingleNode(
                        "/chummer/neighborhoods/neighborhood[name = " + BaseLifestyle.CleanXPath() + ']');
                xmlLifestyleNode.TryGetInt32FieldQuickly("minimum", ref _intBaseArea);
                xmlLifestyleNode.TryGetInt32FieldQuickly("limit", ref _intAreaMaximum);

                // Security.
                xmlLifestyleNode =
                    xmlLifestyleXPathDocument.SelectSingleNode(
                        "/chummer/securities/security[name = " + BaseLifestyle.CleanXPath() + ']');
                xmlLifestyleNode.TryGetInt32FieldQuickly("minimum", ref _intBaseSecurity);
                xmlLifestyleNode.TryGetInt32FieldQuickly("limit", ref _intSecurityMaximum);
                if (_objCharacter.Settings.BookEnabled("HT") || _objCharacter.Settings.AllowFreeGrids)
                {
                    using (XmlNodeList lstGridNodes = objXmlLifestyle.SelectNodes("freegrids/freegrid"))
                    {
                        if (lstGridNodes == null || lstGridNodes.Count <= 0)
                            return;

                        foreach (LifestyleQuality objFreeGrid in LifestyleQualities.Where(x => x.IsFreeGrid).ToList())
                        {
                            objFreeGrid.Remove(false);
                        }

                        XmlDocument xmlLifestyleDocument = _objCharacter.LoadData("lifestyles.xml");
                        foreach (XmlNode xmlNode in lstGridNodes)
                        {
                            XmlNode xmlQuality = xmlLifestyleDocument.SelectSingleNode(
                                "/chummer/qualities/quality[name = " + xmlNode.InnerText.CleanXPath() + ']');
                            LifestyleQuality objQuality = new LifestyleQuality(_objCharacter);
                            string strPush = xmlNode.SelectSingleNode("@select")?.Value;
                            if (!string.IsNullOrWhiteSpace(strPush))
                            {
                                _objCharacter.PushText.Push(strPush);
                            }

                            objQuality.Create(xmlQuality, this, _objCharacter, QualitySource.BuiltIn);
                            objQuality.IsFreeGrid = true;
                            LifestyleQualities.Add(objQuality);
                        }
                    }
                }
            }
        }

        private SourceString _objCachedSourceDetail;

        public SourceString SourceDetail
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_objCachedSourceDetail == default)
                        _objCachedSourceDetail = SourceString.GetSourceString(Source,
                                                                              DisplayPage(GlobalSettings.Language),
                                                                              GlobalSettings.Language,
                                                                              GlobalSettings.CultureInfo,
                                                                              _objCharacter);
                    return _objCachedSourceDetail;
                }
            }
        }

        /// <summary>
        /// Save the object's XML to the XmlWriter.
        /// </summary>
        /// <param name="objWriter">XmlTextWriter to write with.</param>
        public void Save(XmlWriter objWriter)
        {
            if (objWriter == null)
                return;
            using (EnterReadLock.Enter(LockObject))
            {
                objWriter.WriteStartElement("lifestyle");
                objWriter.WriteElementString("sourceid", SourceIDString);
                objWriter.WriteElementString("guid", InternalId);
                objWriter.WriteElementString("name", _strName);
                objWriter.WriteElementString("cost", _decCost.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("dice", _intDice.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("lp", _intLP.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("baselifestyle", _strBaseLifestyle);
                objWriter.WriteElementString("multiplier",
                                             _decMultiplier.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("months", _intIncrements.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("roommates", _intRoommates.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("percentage",
                                             _decPercentage.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("area", _intArea.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("comforts", _intComforts.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("security", _intSecurity.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("basearea", _intBaseArea.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("basecomforts",
                                             _intBaseComforts.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("basesecurity",
                                             _intBaseSecurity.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("maxarea", _intAreaMaximum.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("maxcomforts",
                                             _intComfortsMaximum.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("maxsecurity",
                                             _intSecurityMaximum.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("costforearea",
                                             _decCostForArea.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("costforcomforts",
                                             _decCostForComforts.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("costforsecurity",
                                             _decCostForSecurity.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("allowbonuslp",
                                             _blnAllowBonusLP.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("bonuslp", _intBonusLP.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("source", _strSource);
                objWriter.WriteElementString("page", _strPage);
                objWriter.WriteElementString("trustfund", _blnTrustFund.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("primarytenant",
                                             _blnIsPrimaryTenant.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteElementString("type", _eType.ToString());
                objWriter.WriteElementString("increment", _eIncrement.ToString());
                objWriter.WriteElementString("sourceid", SourceIDString);

                objWriter.WriteElementString("city", _strCity);
                objWriter.WriteElementString("district", _strDistrict);
                objWriter.WriteElementString("borough", _strBorough);

                objWriter.WriteStartElement("lifestylequalities");
                foreach (LifestyleQuality objQuality in LifestyleQualities)
                {
                    objQuality.Save(objWriter);
                }

                objWriter.WriteEndElement();
                objWriter.WriteElementString("notes", _strNotes.CleanOfInvalidUnicodeChars());
                objWriter.WriteElementString("notesColor", ColorTranslator.ToHtml(_colNotes));
                objWriter.WriteElementString("sortorder", _intSortOrder.ToString(GlobalSettings.InvariantCultureInfo));
                objWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Load the CharacterAttribute from the XmlNode.
        /// </summary>
        /// <param name="objNode">XmlNode to load.</param>
        /// <param name="blnCopy"></param>
        public void Load(XmlNode objNode, bool blnCopy = false)
        {
            using (LockObject.EnterWriteLock())
            {
                if (blnCopy || !objNode.TryGetField("guid", Guid.TryParse, out _guiID))
                {
                    _guiID = Guid.NewGuid();
                }

                objNode.TryGetStringFieldQuickly("name", ref _strName);
                _objCachedMyXmlNode = null;
                _objCachedMyXPathNode = null;
                Lazy<XmlNode> objMyNode = new Lazy<XmlNode>(() => this.GetNode());
                if (!objNode.TryGetGuidFieldQuickly("sourceid", ref _guiSourceID))
                {
                    objMyNode.Value?.TryGetGuidFieldQuickly("id", ref _guiSourceID);
                }

                if (blnCopy)
                {
                    _intIncrements = 0;
                }
                else
                {
                    objNode.TryGetInt32FieldQuickly("months", ref _intIncrements);
                    objNode.TryGetField("guid", Guid.TryParse, out _guiID);
                }

                objNode.TryGetDecFieldQuickly("cost", ref _decCost);
                objNode.TryGetInt32FieldQuickly("dice", ref _intDice);
                objNode.TryGetDecFieldQuickly("multiplier", ref _decMultiplier);

                objNode.TryGetStringFieldQuickly("city", ref _strCity);
                objNode.TryGetStringFieldQuickly("district", ref _strDistrict);
                objNode.TryGetStringFieldQuickly("borough", ref _strBorough);

                objNode.TryGetInt32FieldQuickly("area", ref _intArea);
                objNode.TryGetInt32FieldQuickly("comforts", ref _intComforts);
                objNode.TryGetInt32FieldQuickly("security", ref _intSecurity);
                objNode.TryGetInt32FieldQuickly("basearea", ref _intBaseArea);
                objNode.TryGetInt32FieldQuickly("basecomforts", ref _intBaseComforts);
                objNode.TryGetInt32FieldQuickly("basesecurity", ref _intBaseSecurity);
                objNode.TryGetDecFieldQuickly("costforarea", ref _decCostForArea);
                objNode.TryGetDecFieldQuickly("costforcomforts", ref _decCostForComforts);
                objNode.TryGetDecFieldQuickly("costforsecurity", ref _decCostForSecurity);
                objNode.TryGetInt32FieldQuickly("roommates", ref _intRoommates);
                objNode.TryGetDecFieldQuickly("percentage", ref _decPercentage);
                objNode.TryGetStringFieldQuickly("baselifestyle", ref _strBaseLifestyle);
                objNode.TryGetInt32FieldQuickly("sortorder", ref _intSortOrder);
                XPathNavigator xmlLifestyles = _objCharacter.LoadDataXPath("lifestyles.xml");
                if (xmlLifestyles.SelectSingleNode("/chummer/lifestyles/lifestyle[name = " + BaseLifestyle.CleanXPath()
                                                   + ']') == null
                    && xmlLifestyles.SelectSingleNode("/chummer/lifestyles/lifestyle[name =" + Name.CleanXPath() + ']')
                    != null)
                {
                    (_strName, _strBaseLifestyle) = (_strBaseLifestyle, _strName);
                }

                if (string.IsNullOrWhiteSpace(_strBaseLifestyle))
                {
                    objNode.TryGetStringFieldQuickly("lifestylename", ref _strBaseLifestyle);
                    if (string.IsNullOrWhiteSpace(_strBaseLifestyle))
                    {
                        using (new FetchSafelyFromPool<List<ListItem>>(Utils.ListItemListPool,
                                                                       out List<ListItem> lstQualities))
                        {
                            foreach (XPathNavigator xmlLifestyle in xmlLifestyles.SelectAndCacheExpression(
                                         "/chummer/lifestyles/lifestyle"))
                            {
                                string strName = xmlLifestyle.SelectSingleNodeAndCacheExpression("name")?.Value
                                                 ?? LanguageManager.GetString("String_Error");
                                lstQualities.Add(
                                    new ListItem(
                                        strName,
                                        xmlLifestyle.SelectSingleNodeAndCacheExpression("translate")?.Value
                                        ?? strName));
                            }

                            using (ThreadSafeForm<SelectItem> frmSelect = ThreadSafeForm<SelectItem>.Get(
                                       () => new SelectItem
                                       {
                                           Description = string.Format(GlobalSettings.CultureInfo,
                                                                       LanguageManager.GetString(
                                                                           "String_CannotFindLifestyle"),
                                                                       _strName)
                                       }))
                            {
                                frmSelect.MyForm.SetGeneralItemsMode(lstQualities);
                                if (frmSelect.ShowDialogSafe(_objCharacter) == DialogResult.Cancel)
                                {
                                    _guiID = Guid.Empty;
                                    return;
                                }

                                _strBaseLifestyle = frmSelect.MyForm.SelectedItem;
                            }
                        }
                    }
                }

                if (_strBaseLifestyle == "Middle")
                    _strBaseLifestyle = "Medium";
                // Legacy sweep for issues with Advanced Lifestyle selector not properly resetting values upon changes to the Base Lifestyle
                if (_objCharacter.LastSavedVersion <= new Version(5, 212, 73)
                    && _strBaseLifestyle != "Street"
                    && (_decCostForArea != 0 || _decCostForComforts != 0 || _decCostForSecurity != 0))
                {
                    XmlNode xmlDataNode = objMyNode.Value;
                    if (xmlDataNode != null)
                    {
                        xmlDataNode.TryGetDecFieldQuickly("costforarea", ref _decCostForArea);
                        xmlDataNode.TryGetDecFieldQuickly("costforcomforts", ref _decCostForComforts);
                        xmlDataNode.TryGetDecFieldQuickly("costforsecurity", ref _decCostForSecurity);
                    }
                }

                if (!objNode.TryGetBoolFieldQuickly("allowbonuslp", ref _blnAllowBonusLP))
                    objMyNode.Value?.TryGetBoolFieldQuickly("allowbonuslp", ref _blnAllowBonusLP);
                if (!objNode.TryGetInt32FieldQuickly("bonuslp", ref _intBonusLP) && _strBaseLifestyle == "Traveler")
                    _intBonusLP = GlobalSettings.RandomGenerator.NextD6ModuloBiasRemoved();

                if (!objNode.TryGetInt32FieldQuickly("lp", ref _intLP))
                {
                    XPathNavigator xmlLifestyleNode =
                        xmlLifestyles.SelectSingleNode("/chummer/lifestyles/lifestyle[name = "
                                                       + BaseLifestyle.CleanXPath() + ']');
                    xmlLifestyleNode.TryGetInt32FieldQuickly("lp", ref _intLP);
                }

                if (!objNode.TryGetInt32FieldQuickly("maxarea", ref _intAreaMaximum))
                {
                    XPathNavigator xmlLifestyleNode =
                        xmlLifestyles.SelectSingleNode("/chummer/comforts/comfort[name = " + BaseLifestyle.CleanXPath()
                                                       + ']');
                    xmlLifestyleNode.TryGetInt32FieldQuickly("minimum", ref _intBaseComforts);
                    xmlLifestyleNode.TryGetInt32FieldQuickly("limit", ref _intComfortsMaximum);

                    // Area.
                    xmlLifestyleNode =
                        xmlLifestyles.SelectSingleNode("/chummer/neighborhoods/neighborhood[name = "
                                                       + BaseLifestyle.CleanXPath() + ']');
                    xmlLifestyleNode.TryGetInt32FieldQuickly("minimum", ref _intBaseArea);
                    xmlLifestyleNode.TryGetInt32FieldQuickly("limit", ref _intAreaMaximum);

                    // Security.
                    xmlLifestyleNode =
                        xmlLifestyles.SelectSingleNode("/chummer/securities/security[name = "
                                                       + BaseLifestyle.CleanXPath() + ']');
                    xmlLifestyleNode.TryGetInt32FieldQuickly("minimum", ref _intBaseSecurity);
                    xmlLifestyleNode.TryGetInt32FieldQuickly("limit", ref _intSecurityMaximum);
                }
                else
                {
                    objNode.TryGetInt32FieldQuickly("maxarea", ref _intAreaMaximum);
                    objNode.TryGetInt32FieldQuickly("maxcomforts", ref _intComfortsMaximum);
                    objNode.TryGetInt32FieldQuickly("maxsecurity", ref _intSecurityMaximum);
                }

                objNode.TryGetStringFieldQuickly("source", ref _strSource);
                objNode.TryGetBoolFieldQuickly("trustfund", ref _blnTrustFund);
                if (objNode["primarytenant"] == null)
                {
                    _blnIsPrimaryTenant = _intRoommates == 0;
                }
                else
                {
                    objNode.TryGetBoolFieldQuickly("primarytenant", ref _blnIsPrimaryTenant);
                }

                objNode.TryGetStringFieldQuickly("page", ref _strPage);

                // Lifestyle Qualities
                using (XmlNodeList xmlQualityList = objNode.SelectNodes("lifestylequalities/lifestylequality"))
                {
                    if (xmlQualityList != null)
                    {
                        foreach (XmlNode xmlQuality in xmlQualityList)
                        {
                            LifestyleQuality objQuality = new LifestyleQuality(_objCharacter);
                            objQuality.Load(xmlQuality, this);
                            LifestyleQualities.Add(objQuality);
                        }
                    }
                }

                // Legacy sweep:
                // Free Grids provided by the Lifestyle saved to a separate node
                using (XmlNodeList xmlQualityList = objNode.SelectNodes("freegrids/lifestylequality"))
                {
                    if (xmlQualityList != null)
                    {
                        foreach (XmlNode xmlQuality in xmlQualityList)
                        {
                            LifestyleQuality objQuality = new LifestyleQuality(_objCharacter);
                            objQuality.Load(xmlQuality, this);
                            objQuality.IsFreeGrid = true;
                            LifestyleQualities.Add(objQuality);
                        }
                    }
                }

                objNode.TryGetMultiLineStringFieldQuickly("notes", ref _strNotes);

                string sNotesColor = ColorTranslator.ToHtml(ColorManager.HasNotesColor);
                objNode.TryGetStringFieldQuickly("notesColor", ref sNotesColor);
                _colNotes = ColorTranslator.FromHtml(sNotesColor);

                string strTemp = string.Empty;
                if (objNode.TryGetStringFieldQuickly("type", ref strTemp))
                {
                    _eType = ConvertToLifestyleType(strTemp);
                }

                if (objNode.TryGetStringFieldQuickly("increment", ref strTemp))
                {
                    _eIncrement = ConvertToLifestyleIncrement(strTemp);
                }
                else if (_eType == LifestyleType.Safehouse)
                    _eIncrement = LifestyleIncrement.Week;
                else if (objMyNode.Value?.TryGetStringFieldQuickly("increment", ref strTemp) == true)
                    _eIncrement = ConvertToLifestyleIncrement(strTemp);

                LegacyShim(objNode);
            }
        }

        /// <summary>
        /// Converts old lifestyle structures to new standards.
        /// </summary>
        private void LegacyShim(XmlNode xmlLifestyleNode)
        {
            using (LockObject.EnterWriteLock())
            {
                //Lifestyles would previously store the entire calculated value of their Cost, Area, Comforts and Security. Better to have it be a volatile Complex Property.
                if (_objCharacter.LastSavedVersion > new Version(5, 197, 0) ||
                    xmlLifestyleNode["costforarea"] != null) return;
                XPathNavigator objXmlDocument = _objCharacter.LoadDataXPath("lifestyles.xml");
                XPathNavigator objLifestyleQualityNode
                    = objXmlDocument.SelectSingleNode("/chummer/lifestyles/lifestyle[name = "
                                                      + BaseLifestyle.CleanXPath() + ']');
                if (objLifestyleQualityNode != null)
                {
                    decimal decTemp = 0.0m;
                    if (objLifestyleQualityNode.TryGetDecFieldQuickly("cost", ref decTemp))
                        Cost = decTemp;
                    if (objLifestyleQualityNode.TryGetDecFieldQuickly("costforarea", ref decTemp))
                        CostForArea = decTemp;
                    if (objLifestyleQualityNode.TryGetDecFieldQuickly("costforcomforts", ref decTemp))
                        CostForComforts = decTemp;
                    if (objLifestyleQualityNode.TryGetDecFieldQuickly("costforsecurity", ref decTemp))
                        CostForSecurity = decTemp;
                }

                int intMinArea = 0;
                int intMinComfort = 0;
                int intMinSec = 0;
                int intMaxArea = 0;
                int intMaxComfort = 0;
                int intMaxSec = 0;

                // Calculate the limits of the 3 aspects.
                // Area.
                XPathNavigator objXmlNode
                    = objXmlDocument.SelectSingleNode("/chummer/neighborhoods/neighborhood[name = "
                                                      + BaseLifestyle.CleanXPath() + ']');
                objXmlNode.TryGetInt32FieldQuickly("minimum", ref intMinArea);
                objXmlNode.TryGetInt32FieldQuickly("limit", ref intMaxArea);
                BaseArea = intMinArea;
                AreaMaximum = Math.Max(intMaxArea, intMinArea);
                // Comforts.
                objXmlNode = objXmlDocument.SelectSingleNode(
                    "/chummer/comforts/comfort[name = " + BaseLifestyle.CleanXPath() + ']');
                objXmlNode.TryGetInt32FieldQuickly("minimum", ref intMinComfort);
                objXmlNode.TryGetInt32FieldQuickly("limit", ref intMaxComfort);
                BaseComforts = intMinComfort;
                ComfortsMaximum = Math.Max(intMaxComfort, intMinComfort);
                // Security.
                objXmlNode = objXmlDocument.SelectSingleNode(
                    "/chummer/securities/security[name = " + BaseLifestyle.CleanXPath() + ']');
                objXmlNode.TryGetInt32FieldQuickly("minimum", ref intMinSec);
                objXmlNode.TryGetInt32FieldQuickly("limit", ref intMaxSec);
                BaseSecurity = intMinSec;
                SecurityMaximum = Math.Max(intMaxSec, intMinSec);

                xmlLifestyleNode.TryGetInt32FieldQuickly("area", ref intMinArea);
                xmlLifestyleNode.TryGetInt32FieldQuickly("comforts", ref intMinComfort);
                xmlLifestyleNode.TryGetInt32FieldQuickly("security", ref intMinSec);

                // Calculate the cost of Positive Qualities.
                foreach (LifestyleQuality objQuality in LifestyleQualities.Where(
                             x => x.OriginSource != QualitySource.BuiltIn))
                {
                    intMinArea -= objQuality.Area;
                    intMinComfort -= objQuality.Comforts;
                    intMinSec -= objQuality.Security;
                }

                Area = Math.Max(intMinArea - BaseArea, 0);
                Comforts = Math.Max(intMinComfort - BaseComforts, 0);
                Security = Math.Max(intMinSec - BaseSecurity, 0);
            }
        }

        /// <summary>
        /// Print the object's XML to the XmlWriter.
        /// </summary>
        /// <param name="objWriter">XmlTextWriter to write with.</param>
        /// <param name="objCulture">Culture in which to print.</param>
        /// <param name="strLanguageToPrint">Language in which to print</param>
        /// <param name="token">Cancellation token to listen to.</param>
        public async ValueTask Print(XmlWriter objWriter, CultureInfo objCulture, string strLanguageToPrint, CancellationToken token = default)
        {
            if (objWriter == null)
                return;
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                // <lifestyle>
                XmlElementWriteHelper objBaseElement
                    = await objWriter.StartElementAsync("lifestyle", token).ConfigureAwait(false);
                try
                {
                    await objWriter.WriteElementStringAsync("guid", InternalId, token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("sourceid", SourceIDString, token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("name", CustomName, token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("city", City, token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("district", District, token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("borough", Borough, token).ConfigureAwait(false);
                    await objWriter
                          .WriteElementStringAsync(
                              "cost", Cost.ToString(_objCharacter.Settings.NuyenFormat, objCulture),
                              token).ConfigureAwait(false);
                    await objWriter
                          .WriteElementStringAsync("totalmonthlycost",
                                                   (await GetTotalMonthlyCostAsync(token).ConfigureAwait(false))
                                                   .ToString(
                                                       _objCharacter.Settings.NuyenFormat, objCulture), token)
                          .ConfigureAwait(false);
                    await objWriter
                          .WriteElementStringAsync("totalcost",
                                                   (await GetTotalCostAsync(token).ConfigureAwait(false)).ToString(
                                                       _objCharacter.Settings.NuyenFormat, objCulture),
                                                   token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("dice", Dice.ToString(objCulture), token)
                                   .ConfigureAwait(false);
                    await objWriter
                          .WriteElementStringAsync("multiplier",
                                                   Multiplier.ToString(_objCharacter.Settings.NuyenFormat, objCulture),
                                                   token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("months", Increments.ToString(objCulture), token)
                                   .ConfigureAwait(false);
                    await objWriter
                          .WriteElementStringAsync("purchased", Purchased.ToString(GlobalSettings.InvariantCultureInfo),
                                                   token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("type", StyleType.ToString(), token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("increment", IncrementType.ToString(), token)
                                   .ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("sourceid", SourceIDString, token).ConfigureAwait(false);
                    await objWriter.WriteElementStringAsync("bonuslp", BonusLP.ToString(objCulture), token)
                                   .ConfigureAwait(false);
                    string strBaseLifestyle = string.Empty;

                    // Retrieve the Advanced Lifestyle information if applicable.
                    if (!string.IsNullOrEmpty(BaseLifestyle))
                    {
                        XPathNavigator objXmlAspect = await this.GetNodeXPathAsync(token: token).ConfigureAwait(false);
                        if (objXmlAspect != null)
                        {
                            strBaseLifestyle
                                = (await objXmlAspect.SelectSingleNodeAndCacheExpressionAsync("translate", token)
                                                     .ConfigureAwait(false))?.Value
                                  ?? (await objXmlAspect.SelectSingleNodeAndCacheExpressionAsync("name", token)
                                                        .ConfigureAwait(false))?.Value ?? strBaseLifestyle;
                        }
                    }

                    await objWriter.WriteElementStringAsync("baselifestyle", strBaseLifestyle, token)
                                   .ConfigureAwait(false);
                    await objWriter
                          .WriteElementStringAsync("trustfund", TrustFund.ToString(GlobalSettings.InvariantCultureInfo),
                                                   token).ConfigureAwait(false);
                    await objWriter
                          .WriteElementStringAsync(
                              "source",
                              await _objCharacter.LanguageBookShortAsync(Source, strLanguageToPrint, token)
                                                 .ConfigureAwait(false), token).ConfigureAwait(false);
                    await objWriter
                          .WriteElementStringAsync(
                              "page", await DisplayPageAsync(strLanguageToPrint, token).ConfigureAwait(false), token)
                          .ConfigureAwait(false);

                    // <qualities>
                    XmlElementWriteHelper objQualitiesElement
                        = await objWriter.StartElementAsync("qualities", token).ConfigureAwait(false);
                    try
                    {
                        // Retrieve the Qualities for the Advanced Lifestyle if applicable.
                        foreach (LifestyleQuality objQuality in LifestyleQualities)
                        {
                            await objQuality.Print(objWriter, objCulture, strLanguageToPrint, token)
                                            .ConfigureAwait(false);
                        }
                    }
                    finally
                    {
                        // </qualities>
                        await objQualitiesElement.DisposeAsync().ConfigureAwait(false);
                    }

                    if (GlobalSettings.PrintNotes)
                        await objWriter.WriteElementStringAsync("notes", Notes, token).ConfigureAwait(false);
                }
                finally
                {
                    // </lifestyle>
                    await objBaseElement.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        #endregion Constructor, Create, Save, Load, and Print Methods

        #region Properties

        /// <summary>
        /// Internal identifier which will be used to identify this Lifestyle in the Improvement system.
        /// </summary>
        public string InternalId
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _guiID.ToString("D", GlobalSettings.InvariantCultureInfo);
            }
        }

        /// <summary>
        /// Identifier of the object within data files.
        /// </summary>
        public Guid SourceID
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _guiSourceID;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_guiSourceID == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _guiSourceID = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// String-formatted identifier of the <inheritdoc cref="SourceID"/> from the data files.
        /// </summary>
        public string SourceIDString => SourceID.ToString("D", GlobalSettings.InvariantCultureInfo);

        /// <summary>
        /// A custom name for the Lifestyle assigned by the player.
        /// </summary>
        public string Name
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _strName;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _strName, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        public string CustomName
        {
            get => Name;
            set => Name = value;
        }

        /// <summary>
        /// The name of the object as it should be displayed on printouts (translated name only).
        /// </summary>
        public string DisplayNameShort(string strLanguage)
        {
            // Get the translated name if applicable.
            if (strLanguage.Equals(GlobalSettings.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
                return BaseLifestyle;

            using (EnterReadLock.Enter(LockObject))
            {
                return this.GetNodeXPath(strLanguage)?.SelectSingleNodeAndCacheExpression("translate")?.Value
                       ?? BaseLifestyle;
            }
        }

        /// <summary>
        /// The name of the object as it should be displayed on printouts (translated name only).
        /// </summary>
        public async ValueTask<string> DisplayNameShortAsync(string strLanguage, CancellationToken token = default)
        {
            // Get the translated name if applicable.
            if (strLanguage.Equals(GlobalSettings.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
                return BaseLifestyle;

            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                XPathNavigator objNode = await this.GetNodeXPathAsync(strLanguage, token: token).ConfigureAwait(false);
                return objNode != null
                    ? (await objNode.SelectSingleNodeAndCacheExpressionAsync("translate", token).ConfigureAwait(false))
                    ?.Value ?? BaseLifestyle
                    : BaseLifestyle;
            }
        }

        /// <summary>
        /// The name of the object as it should be displayed in lists. Name (Extra).
        /// </summary>
        public string DisplayName(string strLanguage)
        {
            using (EnterReadLock.Enter(LockObject))
            {
                string strReturn = DisplayNameShort(strLanguage);

                if (!string.IsNullOrEmpty(CustomName))
                    strReturn += LanguageManager.GetString("String_Space") + "(\"" + CustomName + "\")";

                return strReturn;
            }
        }

        /// <summary>
        /// The name of the object as it should be displayed in lists. Name (Extra).
        /// </summary>
        public async ValueTask<string> DisplayNameAsync(string strLanguage, CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                string strReturn = await DisplayNameShortAsync(strLanguage, token).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(CustomName))
                    strReturn += await LanguageManager.GetStringAsync("String_Space", token: token)
                                                      .ConfigureAwait(false) + "(\"" + CustomName + "\")";

                return strReturn;
            }
        }

        public string CurrentDisplayName => DisplayName(GlobalSettings.Language);

        public string CurrentDisplayNameShort => DisplayNameShort(GlobalSettings.Language);

        public ValueTask<string> GetCurrentDisplayNameAsync(CancellationToken token = default) => DisplayNameAsync(GlobalSettings.Language, token);

        public ValueTask<string> GetCurrentDisplayNameShortAsync(CancellationToken token = default) => DisplayNameShortAsync(GlobalSettings.Language, token);

        /// <summary>
        /// Sourcebook.
        /// </summary>
        public string Source
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _strSource;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _strSource, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Sourcebook Page Number.
        /// </summary>
        public string Page
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _strPage;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _strPage, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Sourcebook Page Number using a given language file.
        /// Returns Page if not found or the string is empty.
        /// </summary>
        /// <param name="strLanguage">Language file keyword to use.</param>
        /// <returns></returns>
        public string DisplayPage(string strLanguage)
        {
            if (strLanguage.Equals(GlobalSettings.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
                return Page;
            using (EnterReadLock.Enter(LockObject))
            {
                string s = this.GetNodeXPath(strLanguage)?.SelectSingleNodeAndCacheExpression("altpage")?.Value ?? Page;
                return !string.IsNullOrWhiteSpace(s) ? s : Page;
            }
        }

        /// <summary>
        /// Sourcebook Page Number using a given language file.
        /// Returns Page if not found or the string is empty.
        /// </summary>
        /// <param name="strLanguage">Language file keyword to use.</param>
        /// <param name="token">Cancellation token to listen to.</param>
        /// <returns></returns>
        public async ValueTask<string> DisplayPageAsync(string strLanguage, CancellationToken token = default)
        {
            if (strLanguage.Equals(GlobalSettings.DefaultLanguage, StringComparison.OrdinalIgnoreCase))
                return Page;
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                XPathNavigator objNode = await this.GetNodeXPathAsync(strLanguage, token: token).ConfigureAwait(false);
                string s = objNode != null
                    ? (await objNode.SelectSingleNodeAndCacheExpressionAsync("altpage", token: token)
                                    .ConfigureAwait(false))?.Value ?? Page
                    : Page;
                return !string.IsNullOrWhiteSpace(s) ? s : Page;
            }
        }

        /// <summary>
        /// Cost.
        /// </summary>
        public decimal Cost
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _decCost;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_decCost == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _decCost = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Number of dice the character rolls to determine their starting Nuyen.
        /// </summary>
        public int Dice
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intDice;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intDice, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Number the character multiplies the dice roll with to determine their starting Nuyen.
        /// </summary>
        public decimal Multiplier
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _decMultiplier;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_decMultiplier == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _decMultiplier = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Months/Weeks/Days purchased.
        /// </summary>
        public int Increments
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intIncrements;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intIncrements, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether or not the Lifestyle has been Purchased and no longer rented.
        /// </summary>
        public bool Purchased
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return Increments >= IncrementsRequiredForPermanent;
            }
        }

        public int IncrementsRequiredForPermanent
        {
            get
            {
                switch (IncrementType)
                {
                    case LifestyleIncrement.Day:
                        return 3044; // 30.436875 days per month on average * 100 months, rounded up
                    case LifestyleIncrement.Week:
                        return 435; // 4.348125 weeks per month on average * 100 months, rounded up
                    default:
                        return 100;
                }
            }
        }

        /// <summary>
        /// Base Lifestyle.
        /// </summary>
        public string BaseLifestyle
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _strBaseLifestyle;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _strBaseLifestyle, value) == value)
                        return;
                    XmlDocument xmlLifestyleDocument = _objCharacter.LoadData("lifestyles.xml");
                    using (LockObject.EnterWriteLock())
                    {
                        // This needs a handler for translations, will fix later.
                        if (value == "Bolt Hole")
                        {
                            if (LifestyleQualities.All(x => x.Name != "Not a Home"))
                            {
                                XmlNode xmlQuality
                                    = xmlLifestyleDocument.SelectSingleNode(
                                        "/chummer/qualities/quality[name = \"Not a Home\"]");
                                LifestyleQuality objQuality = new LifestyleQuality(_objCharacter);
                                objQuality.Create(xmlQuality, this, _objCharacter, QualitySource.BuiltIn);

                                LifestyleQualities.Add(objQuality);
                            }
                        }
                        else
                        {
                            foreach (LifestyleQuality objNotAHomeQuality in LifestyleQualities
                                                                            .Where(x => x.Name == "Not a Home"
                                                                                || x.Name == "Dug a Hole").ToList())
                                objNotAHomeQuality.Remove(false);
                        }

                        XmlNode xmlLifestyle
                            = xmlLifestyleDocument.SelectSingleNode(
                                "/chummer/lifestyles/lifestyle[name = " + value.CleanXPath() + ']');
                        if (xmlLifestyle != null)
                        {
                            _strBaseLifestyle = string.Empty;
                            _decCost = 0;
                            _intDice = 0;
                            _decMultiplier = 0;
                            _strSource = string.Empty;
                            _strPage = string.Empty;
                            _intLP = 0;
                            _decCostForArea = 0;
                            _decCostForComforts = 0;
                            _decCostForSecurity = 0;
                            _blnAllowBonusLP = false;
                            _eIncrement = LifestyleIncrement.Month;
                            _intBaseComforts = 0;
                            _intComfortsMaximum = 0;
                            _intBaseArea = 0;
                            _intAreaMaximum = 0;
                            _intBaseSecurity = 0;
                            _intSecurityMaximum = 0;
                            Create(xmlLifestyle);
                            this.OnMultiplePropertyChanged(nameof(BaseLifestyle), nameof(Cost), nameof(Dice),
                                                           nameof(Multiplier), nameof(SourceID), nameof(Source),
                                                           nameof(Page), nameof(LP), nameof(CostForArea),
                                                           nameof(CostForComforts), nameof(CostForSecurity),
                                                           nameof(AllowBonusLP), nameof(IncrementType),
                                                           nameof(BaseComforts), nameof(ComfortsMaximum),
                                                           nameof(BaseArea), nameof(AreaMaximum), nameof(BaseSecurity),
                                                           nameof(SecurityMaximum), nameof(LifestyleQualities));
                            return;
                        }
                    }

                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Base Lifestyle Points awarded by the lifestyle.
        /// </summary>
        public int LP
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intLP;
            }
        }

        /// <summary>
        /// Total LP cost of the Lifestyle, including all qualities, roommates, bonus LP, etc.
        /// </summary>
        public int TotalLP
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return LP - Comforts - Area - Security + Roommates + BonusLP - LifestyleQualities.Sum(x => x.LP);
            }
        }

        public async ValueTask<int> GetTotalLPAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return LP - Comforts - Area - Security + Roommates + BonusLP
                       - await LifestyleQualities.SumAsync(x => x.LP, token: token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Free Lifestyle points from Traveler lifestyle.
        /// </summary>
        public int BonusLP
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intBonusLP;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intBonusLP, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        public bool AllowBonusLP
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _blnAllowBonusLP;
            }
        }

        /// <summary>
        /// Advance Lifestyle Comforts.
        /// </summary>
        public int Comforts
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intComforts;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intComforts, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Base level of Comforts.
        /// </summary>
        public int BaseComforts
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intBaseComforts;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intBaseComforts, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Advance Lifestyle Neighborhood Entertainment.
        /// </summary>
        public int BaseArea
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intBaseArea;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intBaseArea, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Advance Lifestyle Security Entertainment.
        /// </summary>
        public int BaseSecurity
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intBaseSecurity;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intBaseSecurity, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Advance Lifestyle Neighborhood.
        /// </summary>
        public int Area
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intArea;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intArea, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Area as accessible by numericupdown bindings.
        /// </summary>
        public decimal BindableArea
        {
            get => Area;
            set => Area = (int)value;
        }

        /// <summary>
        /// Advance Lifestyle Security.
        /// </summary>
        public int Security
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intSecurity;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intSecurity, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Security as accessible by numericupdown bindings.
        /// </summary>
        public decimal BindableSecurity
        {
            get => Security;
            set => Security = (int)value;
        }

        /// <summary>
        /// Comforts as accessible by numericupdown bindings.
        /// </summary>
        public decimal BindableComforts
        {
            get => Comforts;
            set => Comforts = (int)value;
        }

        public int AreaMaximum
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intAreaMaximum;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intAreaMaximum, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        public int ComfortsMaximum
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intComfortsMaximum;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intComfortsMaximum, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        public int SecurityMaximum
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intSecurityMaximum;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intSecurityMaximum, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        public int TotalComfortsMaximum
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return ComfortsMaximum
                           + LifestyleQualities.Sum(x => x.OriginSource != QualitySource.BuiltIn,
                                                    lq => lq.ComfortsMaximum);
                }
            }
        }

        public async ValueTask<int> GetTotalComfortsMaximumAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return ComfortsMaximum
                       + await LifestyleQualities
                               .SumAsync(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.ComfortsMaximum, token)
                               .ConfigureAwait(false);
            }
        }

        public int TotalSecurityMaximum
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return SecurityMaximum
                           + LifestyleQualities.Sum(x => x.OriginSource != QualitySource.BuiltIn,
                                                    lq => lq.SecurityMaximum);
                }
            }
        }

        public async ValueTask<int> GetTotalSecurityMaximumAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return SecurityMaximum
                       + await LifestyleQualities
                               .SumAsync(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.SecurityMaximum, token)
                               .ConfigureAwait(false);
            }
        }

        public int TotalAreaMaximum
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return AreaMaximum
                           + LifestyleQualities.Sum(x => x.OriginSource != QualitySource.BuiltIn,
                                                    lq => lq.AreaMaximum);
                }
            }
        }

        public async ValueTask<int> GetTotalAreaMaximumAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return AreaMaximum
                       + await LifestyleQualities
                               .SumAsync(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.AreaMaximum, token)
                               .ConfigureAwait(false);
            }
        }

        private readonly ThreadSafeObservableCollection<LifestyleQuality> _lstLifestyleQualities = new ThreadSafeObservableCollection<LifestyleQuality>();

        /// <summary>
        /// Advanced Lifestyle Qualities.
        /// </summary>
        public ThreadSafeObservableCollection<LifestyleQuality> LifestyleQualities
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _lstLifestyleQualities;
            }
        }

        /// <summary>
        /// Notes.
        /// </summary>
        public string Notes
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _strNotes;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _strNotes, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Forecolor to use for Notes in treeviews.
        /// </summary>
        public Color NotesColor
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _colNotes;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_colNotes == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _colNotes = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Type of the Lifestyle.
        /// </summary>
        public LifestyleType StyleType
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _eType;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (InterlockedExtensions.Exchange(ref _eType, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Interval of payments required for the Lifestyle.
        /// </summary>
        public LifestyleIncrement IncrementType
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _eIncrement;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (InterlockedExtensions.Exchange(ref _eIncrement, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Number of Roommates this Lifestyle is shared with.
        /// </summary>
        public int Roommates
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intRoommates;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intRoommates, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Percentage of the total cost the character pays per month.
        /// </summary>
        public decimal Percentage
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _decPercentage;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_decPercentage == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _decPercentage = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether the lifestyle is currently covered by the Trust Fund Quality.
        /// </summary>
        public bool TrustFund
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _blnTrustFund && IsTrustFundEligible;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_blnTrustFund == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _blnTrustFund = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsTrustFundEligible => StaticIsTrustFundEligible(_objCharacter, BaseLifestyle);

        public static bool StaticIsTrustFundEligible(Character objCharacter, string strBaseLifestyle)
        {
            switch (objCharacter.TrustFund)
            {
                case 1:
                case 4:
                    return strBaseLifestyle == "Medium";

                case 2:
                    return strBaseLifestyle == "Low";

                case 3:
                    return strBaseLifestyle == "High";
            }
            return false;
        }

        /// <summary>
        /// Whether the character is the primary tenant for the Lifestyle.
        /// </summary>
        public bool PrimaryTenant
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _blnIsPrimaryTenant || Roommates == 0 || TrustFund;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_blnIsPrimaryTenant == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _blnIsPrimaryTenant = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Nuyen cost for each point of upgraded Security. Expected to be zero for lifestyles other than Street.
        /// </summary>
        public decimal CostForArea
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _decCostForArea;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_decCostForArea == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _decCostForArea = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Nuyen cost for each point of upgraded Security. Expected to be zero for lifestyles other than Street.
        /// </summary>
        public decimal CostForComforts
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _decCostForComforts;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_decCostForComforts == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _decCostForComforts = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Nuyen cost for each point of upgraded Security. Expected to be zero for lifestyles other than Street.
        /// </summary>
        public decimal CostForSecurity
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _decCostForSecurity;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (_decCostForSecurity == value)
                        return;
                    using (LockObject.EnterWriteLock())
                        _decCostForSecurity = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Used by our sorting algorithm to remember which order the user moves things to
        /// </summary>
        public int SortOrder
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _intSortOrder;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _intSortOrder, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        private XmlNode _objCachedMyXmlNode;
        private string _strCachedXmlNodeLanguage = string.Empty;

        public async Task<XmlNode> GetNodeCoreAsync(bool blnSync, string strLanguage, CancellationToken token = default)
        {
            // ReSharper disable once MethodHasAsyncOverload
            using (blnSync ? EnterReadLock.Enter(LockObject, token) : await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                XmlNode objReturn = _objCachedMyXmlNode;
                if (objReturn != null && strLanguage == _strCachedXmlNodeLanguage
                                      && !GlobalSettings.LiveCustomData)
                    return objReturn;
                objReturn = (blnSync
                        // ReSharper disable once MethodHasAsyncOverload
                        ? _objCharacter.LoadData("lifestyles.xml", strLanguage, token: token)
                        : await _objCharacter.LoadDataAsync("lifestyles.xml", strLanguage, token: token)
                                             .ConfigureAwait(false))
                    .SelectSingleNode(SourceID == Guid.Empty
                                          ? "/chummer/lifestyles/lifestyle[name = "
                                            + Name.CleanXPath() + ']'
                                          : "/chummer/lifestyles/lifestyle[id = "
                                            + SourceIDString.CleanXPath() + " or id = "
                                            + SourceIDString.ToUpperInvariant()
                                                            .CleanXPath()
                                            + ']');
                _objCachedMyXmlNode = objReturn;
                _strCachedXmlNodeLanguage = strLanguage;
                return objReturn;
            }
        }

        private XPathNavigator _objCachedMyXPathNode;
        private string _strCachedXPathNodeLanguage = string.Empty;

        public async Task<XPathNavigator> GetNodeXPathCoreAsync(bool blnSync, string strLanguage, CancellationToken token = default)
        {
            // ReSharper disable once MethodHasAsyncOverload
            using (blnSync ? EnterReadLock.Enter(LockObject, token) : await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                XPathNavigator objReturn = _objCachedMyXPathNode;
                if (objReturn != null && strLanguage == _strCachedXPathNodeLanguage
                                      && !GlobalSettings.LiveCustomData)
                    return objReturn;
                objReturn = (blnSync
                        // ReSharper disable once MethodHasAsyncOverload
                        ? _objCharacter.LoadDataXPath("lifestyles.xml", strLanguage, token: token)
                        : await _objCharacter.LoadDataXPathAsync("lifestyles.xml", strLanguage, token: token)
                                             .ConfigureAwait(false))
                    .SelectSingleNode(SourceID == Guid.Empty
                                          ? "/chummer/lifestyles/lifestyle[name = "
                                            + Name.CleanXPath() + ']'
                                          : "/chummer/lifestyles/lifestyle[id = "
                                            + SourceIDString.CleanXPath() + " or id = "
                                            + SourceIDString.ToUpperInvariant()
                                                            .CleanXPath()
                                            + ']');
                _objCachedMyXPathNode = objReturn;
                _strCachedXPathNodeLanguage = strLanguage;
                return objReturn;
            }
        }

        /// <summary>
        /// Calculates the Expected Value of an Lifestyle at chargen under the assumption that the average value was rolled
        /// </summary>
        public decimal ExpectedValue
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return 3.5m * Dice * Multiplier;
            }
        }

        public string City
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _strCity;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _strCity, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        public string District
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _strDistrict;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _strDistrict, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        public string Borough
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return _strBorough;
            }
            set
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    if (Interlocked.Exchange(ref _strBorough, value) != value)
                        OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Complex Properties

        /// <summary>
        /// Total cost of the Lifestyle, counting all purchased months.
        /// </summary>
        public decimal TotalCost
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return TotalMonthlyCost * Increments;
            }
        }

        public async ValueTask<decimal> GetTotalCostAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
                return await GetTotalMonthlyCostAsync(token).ConfigureAwait(false) * Increments;
        }

        public decimal CostMultiplier
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    decimal d = (Roommates + Area + Comforts + Security) * 10;
                    d += ImprovementManager.ValueOf(_objCharacter, Improvement.ImprovementType.LifestyleCost, false,
                                                    BaseLifestyle, true, true);
                    if (StyleType == LifestyleType.Standard)
                    {
                        d += ImprovementManager.ValueOf(_objCharacter, Improvement.ImprovementType.BasicLifestyleCost);
                        d += ImprovementManager.ValueOf(_objCharacter, Improvement.ImprovementType.BasicLifestyleCost,
                                                        false, BaseLifestyle);
                    }

                    d += LifestyleQualities.Sum(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.Multiplier);
                    d += 100M;
                    return Math.Max(d / 100, 0);
                }
            }
        }

        public async ValueTask<decimal> GetCostMultiplierAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                decimal d = (Roommates + Area + Comforts + Security) * 10;
                d += await ImprovementManager.ValueOfAsync(_objCharacter, Improvement.ImprovementType.LifestyleCost,
                                                           false,
                                                           BaseLifestyle, true, true, token).ConfigureAwait(false);
                if (StyleType == LifestyleType.Standard)
                {
                    d += await ImprovementManager
                               .ValueOfAsync(_objCharacter, Improvement.ImprovementType.BasicLifestyleCost,
                                             token: token)
                               .ConfigureAwait(false);
                    d += await ImprovementManager.ValueOfAsync(_objCharacter,
                                                               Improvement.ImprovementType.BasicLifestyleCost, false,
                                                               BaseLifestyle, token: token).ConfigureAwait(false);
                }

                d += await LifestyleQualities
                           .SumAsync(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.Multiplier, token: token)
                           .ConfigureAwait(false);
                d += 100M;
                return Math.Max(d / 100, 0);
            }
        }

        /// <summary>
        /// Total Area of the Lifestyle, including all Lifestyle qualities.
        /// </summary>
        public int TotalArea
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return BaseArea + Area
                                    + LifestyleQualities.Sum(x => x.OriginSource != QualitySource.BuiltIn,
                                                             lq => lq.Area);
                }
            }
        }

        /// <summary>
        /// Total Area of the Lifestyle, including all Lifestyle qualities.
        /// </summary>
        public async ValueTask<int> GetTotalAreaAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return BaseArea + Area
                                + await LifestyleQualities.SumAsync(x => x.OriginSource != QualitySource.BuiltIn,
                                                                    lq => lq.Area, token: token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Total Comforts of the Lifestyle, including all Lifestyle qualities.
        /// </summary>
        public int TotalComforts
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return BaseComforts + Comforts
                                        + LifestyleQualities.Sum(
                                            x => x.OriginSource != QualitySource.BuiltIn, lq => lq.Comforts);
                }
            }
        }

        /// <summary>
        /// Total Comforts of the Lifestyle, including all Lifestyle qualities.
        /// </summary>
        public async ValueTask<int> GetTotalComfortsAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return BaseComforts + Comforts
                                    + await LifestyleQualities.SumAsync(x => x.OriginSource != QualitySource.BuiltIn,
                                                                        lq => lq.Comforts, token: token)
                                                              .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Total Security of the Lifestyle, including all Lifestyle qualities.
        /// </summary>
        public int TotalSecurity
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return BaseSecurity + Security
                                        + LifestyleQualities.Sum(
                                            x => x.OriginSource != QualitySource.BuiltIn, lq => lq.Security);
                }
            }
        }

        /// <summary>
        /// Total Security of the Lifestyle, including all Lifestyle qualities.
        /// </summary>
        public async ValueTask<int> GetTotalSecurityAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return BaseSecurity + Security
                                    + await LifestyleQualities.SumAsync(x => x.OriginSource != QualitySource.BuiltIn,
                                                                        lq => lq.Security, token: token)
                                                              .ConfigureAwait(false);
            }
        }

        public decimal AreaDelta
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return Math.Max(
                        TotalAreaMaximum - (BaseArea
                                            + LifestyleQualities.Sum(x => x.OriginSource != QualitySource.BuiltIn,
                                                                     lq => lq.Area)), 0);
                }
            }
        }

        public async ValueTask<decimal> GetAreaDeltaAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return Math.Max(
                    await GetTotalAreaMaximumAsync(token).ConfigureAwait(false) - (BaseArea + await LifestyleQualities
                        .SumAsync(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.Area, token: token)
                        .ConfigureAwait(false)), 0);
            }
        }

        public decimal ComfortsDelta
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return Math.Max(
                        TotalComfortsMaximum - (BaseComforts
                                                + LifestyleQualities.Sum(x => x.OriginSource != QualitySource.BuiltIn,
                                                                         lq => lq.Comforts)), 0);
                }
            }
        }

        public async ValueTask<decimal> GetComfortsDeltaAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return Math.Max(
                    await GetTotalComfortsMaximumAsync(token).ConfigureAwait(false) - (BaseComforts
                        + await LifestyleQualities
                                .SumAsync(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.Comforts, token: token)
                                .ConfigureAwait(false)), 0);
            }
        }

        public decimal SecurityDelta
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return Math.Max(
                        TotalSecurityMaximum - (BaseSecurity
                                                + LifestyleQualities.Sum(x => x.OriginSource != QualitySource.BuiltIn,
                                                                         lq => lq.Security)), 0);
                }
            }
        }

        public async ValueTask<decimal> GetSecurityDeltaAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return Math.Max(
                    await GetTotalSecurityMaximumAsync(token).ConfigureAwait(false) - (BaseSecurity
                        + await LifestyleQualities
                                .SumAsync(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.Security, token: token)
                                .ConfigureAwait(false)), 0);
            }
        }

        public string FormattedArea
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return string.Format(GlobalSettings.CultureInfo,
                                         LanguageManager.GetString("Label_SelectAdvancedLifestyle_Base"),
                                         BaseArea.ToString(GlobalSettings.CultureInfo),
                                         TotalAreaMaximum.ToString(GlobalSettings.CultureInfo));
                }
            }
        }

        public async ValueTask<string> GetFormattedAreaAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return string.Format(
                    GlobalSettings.CultureInfo,
                    await LanguageManager.GetStringAsync("Label_SelectAdvancedLifestyle_Base", token: token)
                                         .ConfigureAwait(false), BaseArea.ToString(GlobalSettings.CultureInfo),
                    (await GetTotalAreaMaximumAsync(token).ConfigureAwait(false)).ToString(GlobalSettings.CultureInfo));
            }
        }

        public string FormattedComforts
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return string.Format(GlobalSettings.CultureInfo,
                                         LanguageManager.GetString("Label_SelectAdvancedLifestyle_Base"),
                                         BaseComforts.ToString(GlobalSettings.CultureInfo),
                                         TotalComfortsMaximum.ToString(GlobalSettings.CultureInfo));
                }
            }
        }

        public async ValueTask<string> GetFormattedComfortsAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return string.Format(
                    GlobalSettings.CultureInfo,
                    await LanguageManager.GetStringAsync("Label_SelectAdvancedLifestyle_Base", token: token)
                                         .ConfigureAwait(false), BaseComforts.ToString(GlobalSettings.CultureInfo),
                    (await GetTotalComfortsMaximumAsync(token).ConfigureAwait(false)).ToString(
                        GlobalSettings.CultureInfo));
            }
        }

        public string FormattedSecurity
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return string.Format(GlobalSettings.CultureInfo,
                                         LanguageManager.GetString("Label_SelectAdvancedLifestyle_Base"),
                                         BaseSecurity.ToString(GlobalSettings.CultureInfo),
                                         TotalSecurityMaximum.ToString(GlobalSettings.CultureInfo));
                }
            }
        }

        public async ValueTask<string> GetFormattedSecurityAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return string.Format(
                    GlobalSettings.CultureInfo,
                    await LanguageManager.GetStringAsync("Label_SelectAdvancedLifestyle_Base", token: token)
                                         .ConfigureAwait(false), BaseSecurity.ToString(GlobalSettings.CultureInfo),
                    (await GetTotalSecurityMaximumAsync(token).ConfigureAwait(false)).ToString(
                        GlobalSettings.CultureInfo));
            }
        }

        /// <summary>
        /// Base cost of the Lifestyle itself, including all multipliers from Improvements, qualities and upgraded attributes.
        /// </summary>
        public decimal BaseCost
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                    return Cost * (CostMultiplier + BaseCostMultiplier);
            }
        }

        /// <summary>
        /// Base cost of the Lifestyle itself, including all multipliers from Improvements, qualities and upgraded attributes.
        /// </summary>
        public async ValueTask<decimal> GetBaseCostAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return Cost * (await GetCostMultiplierAsync(token).ConfigureAwait(false)
                               + await GetBaseCostMultiplierAsync(token).ConfigureAwait(false));
            }
        }

        /// <summary>
        /// Base Cost Multiplier from any Lifestyle Qualities the Lifestyle has.
        /// </summary>
        public decimal BaseCostMultiplier
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return LifestyleQualities.Sum(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.BaseMultiplier)
                           / 100.0m;
                }
            }
        }

        /// <summary>
        /// Base Cost Multiplier from any Lifestyle Qualities the Lifestyle has.
        /// </summary>
        public async ValueTask<decimal> GetBaseCostMultiplierAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return await LifestyleQualities
                             .SumAsync(x => x.OriginSource != QualitySource.BuiltIn, lq => lq.BaseMultiplier,
                                       token: token)
                             .ConfigureAwait(false) / 100.0m;
            }
        }

        /// <summary>
        /// Total monthly cost of the Lifestyle.
        /// </summary>
        public decimal TotalMonthlyCost
        {
            get
            {
                decimal decReturn = 0;

                using (EnterReadLock.Enter(LockObject))
                {
                    if (!TrustFund)
                    {
                        decReturn += BaseCost;
                    }

                    decReturn += Area * CostForArea;
                    decReturn += Comforts * CostForComforts;
                    decReturn += Security * CostForSecurity;

                    decimal decExtraAssetCost = 0;
                    decimal decContractCost = 0;
                    foreach (LifestyleQuality objQuality in LifestyleQualities.Where(
                                 x => x.OriginSource != QualitySource.BuiltIn))
                    {
                        //Add the flat cost from Qualities.
                        if (objQuality.Type == QualityType.Contracts)
                            decContractCost += objQuality.Cost;
                        else
                            decExtraAssetCost += objQuality.Cost;
                    }

                    decReturn += decExtraAssetCost;

                    //Qualities may have reduced the cost below zero. No spooky mansion payouts here, so clamp it to zero or higher.
                    decReturn = Math.Max(decReturn, 0);

                    if (!PrimaryTenant)
                    {
                        decReturn /= Roommates + 1.0m;
                    }

                    decReturn *= Percentage / 100;

                    switch (IncrementType)
                    {
                        case LifestyleIncrement.Day:
                            decContractCost /= (4.34812m * 7);
                            break;

                        case LifestyleIncrement.Week:
                            decContractCost /= 4.34812m;
                            break;
                    }

                    decReturn += decContractCost;
                }

                return decReturn;
            }
        }

        /// <summary>
        /// Total monthly cost of the Lifestyle.
        /// </summary>
        public async ValueTask<decimal> GetTotalMonthlyCostAsync(CancellationToken token = default)
        {
            decimal decReturn = 0;
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                if (!TrustFund)
                {
                    decReturn += await GetBaseCostAsync(token).ConfigureAwait(false);
                }

                decReturn += Area * CostForArea;
                decReturn += Comforts * CostForComforts;
                decReturn += Security * CostForSecurity;

                decimal decExtraAssetCost = 0;
                decimal decContractCost = await LifestyleQualities.SumAsync(objQuality =>
                {
                    if (objQuality.OriginSource != QualitySource.BuiltIn)
                    {
                        //Add the flat cost from Qualities.
                        if (objQuality.Type == QualityType.Contracts)
                            return objQuality.Cost;
                        decExtraAssetCost += objQuality.Cost;
                    }

                    return 0;
                }, token: token).ConfigureAwait(false);

                decReturn += decExtraAssetCost;

                //Qualities may have reduced the cost below zero. No spooky mansion payouts here, so clamp it to zero or higher.
                decReturn = Math.Max(decReturn, 0);

                if (!PrimaryTenant)
                {
                    decReturn /= Roommates + 1.0m;
                }

                decReturn *= Percentage / 100;

                switch (IncrementType)
                {
                    case LifestyleIncrement.Day:
                        decContractCost /= (4.34812m * 7);
                        break;

                    case LifestyleIncrement.Week:
                        decContractCost /= 4.34812m;
                        break;
                }

                decReturn += decContractCost;
            }

            return decReturn;
        }

        public string DisplayTotalMonthlyCost
        {
            get
            {
                using (EnterReadLock.Enter(LockObject))
                {
                    return TotalMonthlyCost.ToString(_objCharacter.Settings.NuyenFormat, GlobalSettings.CultureInfo)
                           + LanguageManager.GetString("String_NuyenSymbol");
                }
            }
        }

        public async ValueTask<string> GetDisplayTotalMonthlyCostAsync(CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                return (await GetTotalMonthlyCostAsync(token).ConfigureAwait(false)).ToString(
                           _objCharacter.Settings.NuyenFormat, GlobalSettings.CultureInfo)
                       + await LanguageManager.GetStringAsync("String_NuyenSymbol", token: token).ConfigureAwait(false);
            }
        }

        public static string GetEquivalentLifestyle(string strLifestyle)
        {
            if (string.IsNullOrEmpty(strLifestyle))
                return string.Empty;
            switch (strLifestyle)
            {
                case "Bolt Hole":
                    return "Squatter";

                case "Traveler":
                    return "Low";

                case "Commercial":
                    return "Medium";

                default:
                    return strLifestyle.StartsWith("Hospitalized", StringComparison.Ordinal) ? "High" : strLifestyle;
            }
        }

        #endregion Complex Properties

        #region Methods

        /// <summary>
        /// Set the InternalId for the Lifestyle. Used when editing an Advanced Lifestyle.
        /// </summary>
        /// <param name="strInternalId">InternalId to set.</param>
        public void SetInternalId(string strInternalId)
        {
            if (Guid.TryParse(strInternalId, out Guid guiTemp))
            {
                using (LockObject.EnterWriteLock())
                    _guiID = guiTemp;
            }
        }

        /// <summary>
        /// Purchases an additional month of the selected lifestyle.
        /// </summary>
        public void IncrementMonths()
        {
            using (EnterReadLock.Enter(LockObject))
            {
                // Create the Expense Log Entry.
                decimal decAmount = TotalMonthlyCost;
                if (decAmount > _objCharacter.Nuyen)
                {
                    Program.ShowScrollableMessageBox(LanguageManager.GetString("Message_NotEnoughNuyen"),
                                                     LanguageManager.GetString("MessageTitle_NotEnoughNuyen"),
                                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ExpenseLogEntry objExpense = new ExpenseLogEntry(_objCharacter);
                objExpense.Create(decAmount * -1,
                                  LanguageManager.GetString("String_ExpenseLifestyle") + ' ' + CurrentDisplayNameShort,
                                  ExpenseType.Nuyen, DateTime.Now);
                _objCharacter.ExpenseEntries.AddWithSort(objExpense);
                _objCharacter.Nuyen -= decAmount;

                ExpenseUndo objUndo = new ExpenseUndo();
                objUndo.CreateNuyen(NuyenExpenseType.IncreaseLifestyle, InternalId);
                objExpense.Undo = objUndo;

                Interlocked.Increment(ref _intIncrements);
            }
        }

        private void LifestyleQualitiesOnBeforeClearCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (LifestyleQuality objQuality in e.OldItems)
            {
                objQuality.Dispose();
            }
        }

        private void LifestyleQualitiesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (LifestyleQuality objQuality in e.OldItems)
                    {
                        objQuality.Dispose();
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    HashSet<LifestyleQuality> setNewLifestyleQualities = e.NewItems.OfType<LifestyleQuality>().ToHashSet();
                    foreach (LifestyleQuality objQuality in e.OldItems)
                    {
                        if (!setNewLifestyleQualities.Contains(objQuality))
                            objQuality.Dispose();
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    return;
            }
            OnPropertyChanged(nameof(LifestyleQualities));
        }

        #region UI Methods

        public TreeNode CreateTreeNode(ContextMenuStrip cmsBasicLifestyle, ContextMenuStrip cmsAdvancedLifestyle)
        {
            using (EnterReadLock.Enter(LockObject))
            {
                //if (!string.IsNullOrEmpty(ParentID) && !string.IsNullOrEmpty(Source) && !_objCharacter.Settings.BookEnabled(Source))
                //return null;
                TreeNode objNode = new TreeNode
                {
                    Name = InternalId,
                    Text = CurrentDisplayName,
                    Tag = this,
                    ContextMenuStrip = StyleType == LifestyleType.Standard ? cmsBasicLifestyle : cmsAdvancedLifestyle,
                    ForeColor = PreferredColor,
                    ToolTipText = Notes.WordWrap()
                };
                return objNode;
            }
        }

        public Color PreferredColor =>
            !string.IsNullOrEmpty(Notes)
                ? ColorManager.GenerateCurrentModeColor(NotesColor)
                : ColorManager.WindowText;

        #endregion UI Methods

        #endregion Methods

        private static readonly PropertyDependencyGraph<Lifestyle> s_LifestyleDependencyGraph =
            new PropertyDependencyGraph<Lifestyle>(
                new DependencyGraphNode<string, Lifestyle>(nameof(AreaDelta),
                    new DependencyGraphNode<string, Lifestyle>(nameof(TotalAreaMaximum),
                        new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(AreaMaximum))
                    ),
                    new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseArea))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(TotalArea),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseArea)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Area)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(FormattedArea),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseArea)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(TotalAreaMaximum))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(ComfortsDelta),
                    new DependencyGraphNode<string, Lifestyle>(nameof(TotalComfortsMaximum),
                        new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(ComfortsMaximum))
                    ),
                    new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseComforts))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(TotalComforts),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseComforts)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Comforts)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(FormattedComforts),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseComforts)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(TotalComfortsMaximum))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(SecurityDelta),
                    new DependencyGraphNode<string, Lifestyle>(nameof(TotalSecurityMaximum),
                        new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(SecurityMaximum))
                    ),
                    new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseSecurity))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(TotalSecurity),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseSecurity)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Security)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(FormattedSecurity),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseSecurity)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(TotalAreaMaximum))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(DisplayTotalMonthlyCost),
                    new DependencyGraphNode<string, Lifestyle>(nameof(TotalMonthlyCost),
                        new DependencyGraphNode<string, Lifestyle>(nameof(TrustFund),
                            new DependencyGraphNode<string, Lifestyle>(nameof(IsTrustFundEligible),
                                new DependencyGraphNode<string, Lifestyle>(nameof(BaseLifestyle))
                            )
                        ),
                        new DependencyGraphNode<string, Lifestyle>(nameof(BaseCost), x => x.TrustFund,
                            new DependencyGraphNode<string, Lifestyle>(nameof(TrustFund))
                        ),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Area)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(CostForArea)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Comforts)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(CostForComforts)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Security)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(CostForSecurity)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(PrimaryTenant),
                            new DependencyGraphNode<string, Lifestyle>(nameof(Roommates)),
                            new DependencyGraphNode<string, Lifestyle>(nameof(TrustFund))
                        ),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Roommates), x => !x.PrimaryTenant,
                            new DependencyGraphNode<string, Lifestyle>(nameof(PrimaryTenant))
                        ),
                        new DependencyGraphNode<string, Lifestyle>(nameof(IncrementType)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Percentage))
                    )
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(BaseCost),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Cost)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(CostMultiplier),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Roommates)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Area)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Comforts)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Security)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(StyleType)),
                        new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities))
                    ),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BaseCostMultiplier),
                        new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities))
                    )
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(TotalCost),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Increments)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(TotalMonthlyCost))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(TotalLP),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Comforts)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Area)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Security)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Roommates)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(BonusLP)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(LifestyleQualities))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(ExpectedValue),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Dice)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Multiplier))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(Purchased),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Increments)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(IncrementsRequiredForPermanent),
                        new DependencyGraphNode<string, Lifestyle>(nameof(IncrementType))
                    )
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(CurrentDisplayName),
                    new DependencyGraphNode<string, Lifestyle>(nameof(DisplayName),
                        new DependencyGraphNode<string, Lifestyle>(nameof(CustomName),
                            new DependencyGraphNode<string, Lifestyle>(nameof(Name))
                        ),
                        new DependencyGraphNode<string, Lifestyle>(nameof(DisplayNameShort),
                            new DependencyGraphNode<string, Lifestyle>(nameof(BaseLifestyle))
                        )
                    )
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(SourceIDString),
                    new DependencyGraphNode<string, Lifestyle>(nameof(SourceID))
                ),
                new DependencyGraphNode<string, Lifestyle>(nameof(SourceDetail),
                    new DependencyGraphNode<string, Lifestyle>(nameof(Source)),
                    new DependencyGraphNode<string, Lifestyle>(nameof(DisplayPage),
                        new DependencyGraphNode<string, Lifestyle>(nameof(Page))
                    )
                )
            );

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string strPropertyName = null)
        {
            this.OnMultiplePropertyChanged(strPropertyName);
        }

        public void OnMultiplePropertyChanged(IReadOnlyCollection<string> lstPropertyNames)
        {
            using (EnterReadLock.Enter(LockObject))
            {
                HashSet<string> setNamesOfChangedProperties = null;
                try
                {
                    foreach (string strPropertyName in lstPropertyNames)
                    {
                        if (setNamesOfChangedProperties == null)
                            setNamesOfChangedProperties
                                = s_LifestyleDependencyGraph.GetWithAllDependents(this, strPropertyName, true);
                        else
                        {
                            foreach (string strLoopChangedProperty in s_LifestyleDependencyGraph
                                         .GetWithAllDependentsEnumerable(this, strPropertyName))
                                setNamesOfChangedProperties.Add(strLoopChangedProperty);
                        }
                    }

                    if (setNamesOfChangedProperties == null || setNamesOfChangedProperties.Count == 0)
                        return;

                    if (setNamesOfChangedProperties.Contains(nameof(BaseLifestyle)))
                    {
                        foreach (LifestyleQuality objQuality in LifestyleQualities)
                        {
                            objQuality.OnPropertyChanged(nameof(LifestyleQuality.CanBeFreeByLifestyle));
                        }
                    }

                    if (PropertyChanged != null)
                    {
                        Utils.RunOnMainThread(() =>
                        {
                            if (PropertyChanged != null)
                            {
                                // ReSharper disable once AccessToModifiedClosure
                                foreach (string strPropertyToChange in setNamesOfChangedProperties)
                                {
                                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(strPropertyToChange));
                                }
                            }
                        });
                    }
                }
                finally
                {
                    if (setNamesOfChangedProperties != null)
                        Utils.StringHashSetPool.Return(ref setNamesOfChangedProperties);
                }
            }
        }

        public bool Remove(bool blnConfirmDelete = true)
        {
            using (EnterReadLock.Enter(LockObject))
            {
                if (blnConfirmDelete
                    && !CommonFunctions.ConfirmDelete(LanguageManager.GetString("Message_DeleteLifestyle")))
                    return false;
                using (EnterReadLock.Enter(_objCharacter.Lifestyles.LockObject))
                {
                    if (_objCharacter.Lifestyles.Contains(this) && !_objCharacter.Lifestyles.Remove(this))
                        return false;
                }
            }

            Dispose();
            return true;
        }

        public async ValueTask<bool> RemoveAsync(bool blnConfirmDelete = true, CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                if (blnConfirmDelete && !await CommonFunctions.ConfirmDeleteAsync(
                        await LanguageManager.GetStringAsync("Message_DeleteLifestyle", token: token)
                                             .ConfigureAwait(false), token).ConfigureAwait(false))
                    return false;
                using (await EnterReadLock.EnterAsync(_objCharacter.Lifestyles.LockObject, token).ConfigureAwait(false))
                {
                    if (await _objCharacter.Lifestyles.ContainsAsync(this, token).ConfigureAwait(false)
                        && !await _objCharacter.Lifestyles.RemoveAsync(this, token).ConfigureAwait(false))
                        return false;
                }
            }

            await DisposeAsync().ConfigureAwait(false);
            return true;
        }

        public void SetSourceDetail(Control sourceControl)
        {
            using (EnterReadLock.Enter(LockObject))
            {
                if (_objCachedSourceDetail.Language != GlobalSettings.Language)
                    _objCachedSourceDetail = default;
                SourceDetail.SetControl(sourceControl);
            }
        }

        public async Task SetSourceDetailAsync(Control sourceControl, CancellationToken token = default)
        {
            using (await EnterReadLock.EnterAsync(LockObject, token).ConfigureAwait(false))
            {
                if (_objCachedSourceDetail.Language != GlobalSettings.Language)
                    _objCachedSourceDetail = default;
                await SourceDetail.SetControlAsync(sourceControl, token).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            using (LockObject.EnterWriteLock())
            {
                foreach (LifestyleQuality objQuality in LifestyleQualities)
                    objQuality.Dispose();
                LifestyleQualities.CollectionChanged -= LifestyleQualitiesCollectionChanged;
                LifestyleQualities.BeforeClearCollectionChanged -= LifestyleQualitiesOnBeforeClearCollectionChanged;
                LifestyleQualities.Dispose();
            }

            LockObject.Dispose();
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            IAsyncDisposable objLocker = await LockObject.EnterWriteLockAsync().ConfigureAwait(false);
            try
            {
                foreach (LifestyleQuality objQuality in LifestyleQualities)
                    await objQuality.DisposeAsync().ConfigureAwait(false);
                LifestyleQualities.CollectionChanged -= LifestyleQualitiesCollectionChanged;
                LifestyleQualities.BeforeClearCollectionChanged -= LifestyleQualitiesOnBeforeClearCollectionChanged;
                await LifestyleQualities.DisposeAsync().ConfigureAwait(false);
            }
            finally
            {
                await objLocker.DisposeAsync().ConfigureAwait(false);
            }

            await LockObject.DisposeAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public AsyncFriendlyReaderWriterLock LockObject { get; } = new AsyncFriendlyReaderWriterLock();
    }
}
