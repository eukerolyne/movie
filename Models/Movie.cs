using System.ComponentModel.DataAnnotations;

namespace movie.Models
{
    public class Movie
    {
        [Key]
        public int VideoId { get; set; }

        [Required]
        public string Autor { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string LocalGravacao { get; set; }

        [Required]
        public string TipoVideo { get; set; }

        [Required]
        public string Extensao { get; set; }

        [Required]
        public string Duracao { get; set; }

        [Required]
        public string Assunto { get; set; }

        [Required]
        public string Descricao { get; set; }
    }
}
