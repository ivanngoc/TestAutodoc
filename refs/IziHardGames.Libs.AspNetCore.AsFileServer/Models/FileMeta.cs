using System.ComponentModel.DataAnnotations;

namespace IziHardGames.Libs.AspNetCore.AsFileServer
{
    /// <summary>
    /// File's meta data
    /// </summary>
    public class FileMeta
    {
        /// <summary>
        /// </summary>
        /// <example>somedoc.txt</example>
        [Required]
        public string? fullName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>1024</example>
        [Required]
        public uint length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example><code>DateTime.Now.ToString()</code></example>
        [Required]
        public DateTime? create { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example><code>DateTime.Now.ToString()</code></example>
        [Required]
        public DateTime? write { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>c4955842-8948-424e-85c6-5477e8b50616</example>
        public Guid? guid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>1</example>
        [Required]
        public uint taskId { get; set; }
    }
}
