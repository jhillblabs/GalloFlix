using System.ComponentModel.DataAnnotations;

namespace GalloFlix.ViewModels;

public class RegisterVM
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Por favor, informe o Nome")]
    public string Name { get; set; }

    [Display(Name = "Email do Usuário")]
    [Required(ErrorMessage = "Por favor, informe seu email")]
    public string Email { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data de Nascimento")]
    [Required(ErrorMessage = "Por favor, informe a Data de Nascimento")]
    public DateTime Birthday { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Senha de Acesso")]
    [Required(ErrorMessage = "Por favor, informe sua senha")]
    public string Password { get; set; }
}