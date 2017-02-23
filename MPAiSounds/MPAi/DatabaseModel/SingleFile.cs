namespace MPAi.DatabaseModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Entity class for mapping a file into the database.
    /// </summary>
    [Table("SingleFile")]
    public partial class SingleFile
    {
        /// <summary>
        /// Unique identifier for each file.
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SingleFileId { get; set; }
        /// <summary>
        /// Unique name for each file.
        /// </summary>
        [Required]
        [StringLength(64)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        /// <summary>
        /// A column representing the path to the file.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string Address { get; set; }
        /// <summary>
        /// MANY-TO-ONE RELATIONSHIP
        /// SingleFile (Audio)-Recording
        /// 
        /// Each recording can have several audio files associated with it.
        /// </summary>
        public virtual Recording Audio { get; set; }
        /// <summary>
        /// ONE-TO-ONE RELATIONSHIP
        /// SingleFile (Video)-Recording
        /// 
        /// Each recording can have a single video file associated with it.
        /// </summary>
        public virtual Recording Video { get; set; }
        /// <summary>
        /// ONE-TO-ONE RELATIONSHIP
        /// SingleFile (Vocal Tract)-Recording
        /// 
        /// Each recording can have a single vocal tract file associated with it.
        /// </summary>
        public virtual Recording VocalTract { get; set; }
    }
}
