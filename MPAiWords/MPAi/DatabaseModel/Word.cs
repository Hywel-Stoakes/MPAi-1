namespace MPAi.DatabaseModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Entity class for mapping a word into the database.
    /// </summary>
    [Table("Word")]
    public partial class Word
    {
        /// <summary>
        /// Unique identifier for each word.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WordId { get; set; }
        /// <summary>
        /// Unique name for each word.
        /// </summary>
        [Required]
        [StringLength(64)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        /// <summary>
        /// MANY-TO-ONE RELATIONSHIP
        /// Word-Category
        /// 
        /// Each word has a corresponding category, but we don't store the category in the database.
        /// Instead, we store the ID of the corresponding category in the CategoryID column.
        /// The framework puts them back together with these fields.
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        /// <summary>
        /// ONE-TO-MANY RELATIONSHIP
        /// Word-Recording
        /// 
        /// Each word can have a collection of associated recordings.
        /// </summary>
        public virtual ICollection<Recording> Recordings { get; set; }
    }
}
