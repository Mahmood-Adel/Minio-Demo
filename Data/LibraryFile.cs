using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minio_Demo.Data;

[Table("library_files")]
public class LibraryFile
{
    [Column("id")] public Guid Id { get; set; }
    [Column("name_ar")] [MaxLength(1024)] public string NameAr { get; set; }
    [Column("name_en")] [MaxLength(1024)] public string NameEn { get; set; }
    [Column("file_name")] public string FileName { get; set; }
    [Column("file_size")] public long? FileSize { get; set; }
    [Column("content_type")] public string ContentType { get; set; }
}