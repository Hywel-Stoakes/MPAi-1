namespace MPAi.DatabaseModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Entity class for mapping a recording into the database.
    /// </summary>
    [Table("Recording")]
    public partial class Recording
    {
        /// <summary>
        /// Unique identifier for each recording.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordingId { get; set; }
        /// <summary>
        /// Unique name for each recording.
        /// </summary>
        [Required]
        [StringLength(64)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        /// <summary>
        /// MANY-TO-ONE RELATIONSHIP
        /// Recording-Speaker
        /// 
        /// Each recording has a corresponding speaker, but we don't store the speaker in the database.
        /// Instead, we store the ID of the corresponding speaker in the SpeakerID column.
        /// The framework puts them back together with these fields.
        /// </summary>
        [ForeignKey("SpeakerId")]
        public virtual Speaker Speaker { get; set; }
        public int SpeakerId { get; set; }
        /// <summary>
        /// MANY-TO-ONE RELATIONSHIP
        /// Recording-Word
        /// 
        /// Each recording has a corresponding word, but we don't store the word in the database.
        /// Instead, we store the ID of the corresponding word in the WordID column.
        /// The framework puts them back together with these fields.
        /// </summary>
        [ForeignKey("WordId")]
        public virtual Word Word { get; set; }
        public int WordId { get; set; }
        /// <summary>
        /// ONE-TO-MANY RELATIONSHIP
        /// Recording-SingleFile (Audio)
        /// 
        /// Each recording can have a collection of associated audio files.
        /// </summary>
        public virtual ICollection<SingleFile> Audios { get; set; }
        /// <summary>
        /// ONE-TO-ONE RELATIONSHIP
        /// Recording-SingleFile (Video)
        /// 
        /// Each recording can have a single video attached.
        /// </summary>
        public virtual SingleFile Video { get; set; }
        /// <summary>
        /// ONE-TO-ONE RELATIONSHIP
        /// Recording-SingleFile (Video)
        /// 
        /// Each recording can have a single vocal tract video attached.
        /// </summary>
        public virtual SingleFile VocalTract { get; set; }
    }
}
