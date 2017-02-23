namespace MPAi.DatabaseModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Entity class for mapping a speaker into the database.
    /// </summary>
    [Table("Speaker")]
    public partial class Speaker
    {
        /// <summary>
        /// Unique identifier for each speaker.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpeakerId { get; set; }
        /// <summary>
        /// Unique name for each speaker.
        /// </summary>
        [Required]
        [StringLength(64)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        /// <summary>
        /// ONE-TO-MANY RELATIONSHIP
        /// Speaker-Recording
        /// 
        /// Each speaker can be associated with a collection of recordings.
        /// </summary>
        public virtual ICollection<Recording> Recordings { get; set; }
    }
}
