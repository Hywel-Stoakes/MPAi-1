namespace MPAi.DatabaseModel
{
    using Cores;
    using Modules;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class handling the persistence contexts and object persistence for MPAi.
    /// </summary>
    public partial class MPAiModel : DbContext
    {
        /// <summary>
        /// Default constructor.
        /// Sets the domain for the data base to work inside, that specified in the AppDataPath class, and initialises the database.
        /// </summary>
        public MPAiModel()
            : base("name=MPAiModel")
        {
            // Deprecated: The AppDataPath class holds this value, making it easier to change.
            //AppDomain.CurrentDomain.SetData("DataDirectory", System.Windows.Forms.Application.StartupPath + @"\App_Data");
            if (!Directory.Exists(Path.Combine(AppDataPath.Path, "Database")))
            {
                Directory.CreateDirectory(Path.Combine(AppDataPath.Path, "Database"));
            }
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDataPath.Path, "Database"));

            Database.SetInitializer<MPAiModel>(new MPAiModelInitializer());
        }
        // Variables representing the set of values taken out of the database.
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Recording> Recording { get; set; }
        public virtual DbSet<Speaker> Speaker { get; set; }
        public virtual DbSet<Word> Word { get; set; }
        public virtual DbSet<SingleFile> SingleFile { get; set; }

        public static MPAiModel InitializeDBModel()
        {
            MPAiModel DBModel;
            DBModel = new MPAiModel();
            DBModel.Database.Initialize(false);
            DBModel.Recording.Load();
            DBModel.Speaker.Load();
            DBModel.Category.Load();
            DBModel.Word.Load();
            DBModel.SingleFile.Load();
            return DBModel;
        }

        public bool IsSpeakerString(string str)
        {
            foreach(Speaker speaker in Speaker)
            {
                if(str.Equals(speaker.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsCategoryString(string str)
        {
            foreach (Category category in Category)
            {
                if (str.Equals(category.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsWordString(string str)
        {
            foreach (Word word in Word)
            {
                if (str.Equals(word.Name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Override used to configure some database settings before the database is linked to the context object.
        /// Relations between classes and the nature of such relations are defined here,
        /// as well as which fields are to be used as foreign keys.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the database model being used to create a persistence context.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Deprecated: The code no longer uses separate video and audio recording classes.

            //modelBuilder.Entity<AudioRecording>()
            //    .HasMany(a => a.Audio)
            //    .WithRequired(c => c.Audio)
            //    .Map(m => m.MapKey("AudioId"));

            //modelBuilder.Entity<VideoRecording>()
            //    .HasOptional(v => v.Video)
            //    .WithOptionalDependent(c => c.Video)
            //    .Map(m => m.MapKey("VideoId"));

            modelBuilder.Entity<Recording>()
                .HasMany(r => r.Audios)
                .WithOptional(s => s.Audio)
                .Map(m => m.MapKey("AudioId"));

            modelBuilder.Entity<Recording>()
                .HasOptional(r => r.Video)
                .WithOptionalDependent(s => s.Video)
                .Map(m => m.MapKey("VideoId"));

            modelBuilder.Entity<Recording>()
                .HasOptional(r => r.VocalTract)
                .WithOptionalDependent(s => s.VocalTract)
                .Map(m => m.MapKey("VocalTractId"));

            base.OnModelCreating(modelBuilder); // Continues creating the persistence context.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordingFile"></param>
        public void AddOrUpdateRecordingFile(String recordingFile)
        {
            try
            {
                NameParser parser = new NameParser();
                parser.SingleFileInitialise( recordingFile);

                Speaker spk = this.Speaker.SingleOrDefault(x => x.Name == parser.Speaker);
                if (spk == null)
                {
                    spk = new Speaker()
                    {
                        Name = parser.Speaker
                    };
                    this.Speaker.AddOrUpdate(x => x.Name, spk);
                    this.SaveChanges();
                }

                Category cty = this.Category.SingleOrDefault(x => x.Name == parser.Category);
                if (cty == null)
                {
                    cty = new Category()
                    {
                        // Vocal tract videos are of vowel sounds, and so need to be categorised as such.
                        // However, the category section of the filename is also used to differentiate between videos and vocal tracts.
                        Name = parser.Category.Equals("vocaltract") ? "vowel" : parser.Category
                    };
                    this.Category.AddOrUpdate(x => x.Name, cty);
                    this.SaveChanges();
                }

                Word word = this.Word.SingleOrDefault(x => x.Name == parser.Word);
                if (word == null)
                {
                    word = new Word()
                    {
                        Name = parser.Word,
                        CategoryId = cty.CategoryId
                    };
                    this.Word.AddOrUpdate(x => x.Name, word);
                    this.SaveChanges();
                }

                Recording rd = this.Recording.SingleOrDefault(x => x.Name == parser.Recording); ;
                if (rd == null)
                {
                    rd = new Recording()
                    {
                        Name = parser.Recording,
                        SpeakerId = spk.SpeakerId,
                        WordId = word.WordId
                    };
                    this.Recording.AddOrUpdate(x => x.Name, rd);
                    this.SaveChanges();
                }

                SingleFile sf = this.SingleFile.SingleOrDefault(x => x.Name == parser.FullName);
                if (sf == null)
                {
                    sf = new SingleFile()
                    {
                        Name = parser.FullName,
                        Address = parser.Address,
                    };
                    if (parser.MediaFormat.Equals("audio"))
                    {
                        sf.Audio = rd;
                    }
                    else if (parser.MediaFormat.Equals("video"))
                    {
                        sf.Video = rd;
                    }
                    else if (parser.MediaFormat.Equals("vocaltract"))
                    {
                        sf.VocalTract = rd;
                    }
                    this.SingleFile.AddOrUpdate(x => x.Name, sf);
                    this.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
    }

    /// <summary>
    /// Implementation of a database context initialiser that sets up a database based on the files in the Audio folder. 
    /// </summary>
    public class MPAiModelInitializer : CreateDatabaseIfNotExists<MPAiModel>
    {
        /// <summary>
        /// If the database doesn't exist, it is created.
        /// If it does exist, and the Audio folder a) exists and b) contains at least one .wav file, then
        /// Each .wav file is added to the database, or updated if it is already in the database. 
        /// </summary>
        /// <param name="context">The current MPAiModel object representing the persistence context.</param>
        protected override void Seed(MPAiModel context)
        {
            if(Directory.Exists(DirectoryManagement.AudioFolder))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(DirectoryManagement.AudioFolder);
                foreach(FileInfo fInfo in dirInfo.GetFiles("*.wav", SearchOption.AllDirectories))   // Also searches subdirectories.
                {
                    if(fInfo.Extension.Contains("wav"))
                    {
                        context.AddOrUpdateRecordingFile(Path.Combine(fInfo.DirectoryName, fInfo.FullName));
                    }
                }
            }
            base.Seed(context); // Does nothing. There is no Audio folder, so nothing to add or update.
        }
    }
}
