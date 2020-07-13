using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;

namespace MSLibrary.Survey.DAL
{
    /// <summary>
    /// Survey响应日志
    /// SurveyType+SurveyID+ResponseID唯一
    /// SurveyType为SurveyResponseCollectorEndpoint中的Type
    /// </summary>
    public class SurveyResponseLog:ModelBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }

        /// <summary>
        /// Survey类型
        /// </summary>
        public string SurveyType
        {
            get
            {

                return GetAttribute<string>(nameof(SurveyType));
            }
            set
            {
                SetAttribute<string>(nameof(SurveyType), value);
            }
        }


        /// <summary>
        /// Survey标识
        /// </summary>
        public string SurveyID
        {
            get
            {

                return GetAttribute<string>(nameof(SurveyID));
            }
            set
            {
                SetAttribute<string>(nameof(SurveyID), value);
            }
        }

        /// <summary>
        /// 响应标识
        /// </summary>
        public string ResponseID
        {
            get
            {

                return GetAttribute<string>(nameof(ResponseID));
            }
            set
            {
                SetAttribute<string>(nameof(ResponseID), value);
            }
        }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }
    }
}
