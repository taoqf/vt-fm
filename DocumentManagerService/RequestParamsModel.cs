using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentManagerService
{
    public class RequestParamsModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// FileName
        /// </summary>
        [JsonProperty(PropertyName = "filename")]
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// ContentType
        /// </summary>
        [JsonProperty(PropertyName = "contentType")]
        public string ContentType
        {
            get;
            set;
        }
        /// <summary>
        /// Length
        /// </summary>
        [JsonProperty(PropertyName = "length")]
        public int Length
        {
            get;
            set;
        }
        /// <summary>
        /// ChunkSize
        /// </summary>
        [JsonProperty(PropertyName = "chunkSize")]
        public int ChunkSize
        {
            get;
            set;
        }
        /// <summary>
        /// UploadDate
        /// </summary>
        [JsonProperty(PropertyName = "uploadDate")]
        public string UploadDate
        {
            get;
            set;
        }
        /// <summary>
        /// MD5
        /// </summary>
        [JsonProperty(PropertyName = "md5")]
        public string MD5
        {
            get;
            set;
        }
    }
}
