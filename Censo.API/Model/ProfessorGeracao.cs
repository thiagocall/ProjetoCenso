using System.Collections.Generic;

namespace Censo.API.Model
{
    public class ProfessorGeracao
    {
        public long cpfProfessor { get; set; }
        public string Codies { get; set; }  
        public long CodEmec { get; set; }
        public string NomeCompleto { get; set; }
        public string Dtnascimento { get; set; }
        public string NomSexo { get; set; }
        public string NomRaca { get; set; }
        public string NomMae { get; set; }
        public string NacioProfessor { get; set; }
        public string Pais { get; set; }
        public string UF { get; set; }
        public string Municipio { get; set; }
        public string NomeCurso { get; set; }    
        public string Escolaridade { get; set; } 
        public string DocentecomDeficiencia { get; set; }   
        public string def1 { get; set; }    
        public string def2 { get; set; }    
        public string def3 { get; set; }    
        public string Situacaodocente { get; set; }    
        public string Perfil { get; set; }    
        public string Regime { get; set; }    
        public string DocenteSubstituto { get; set; }
        public string DocenteAtivo3112 { get; set; }
        public string Titulacao { get; set; } 
        public List<IES> Listaies = new List<IES>();
        
    }    

    public class CursoProf
    {
            public long codcursoEmec { get; set; }
            public string nomcursoEmec { get; set; } 
            public string codcursonomecurso { get 
            {
                return this.nomcursoEmec + ";" + this.codcursoEmec.ToString();
            }  
            
            }
            
    }
        
    public class IES
    {
            public string codies { get; set; }
            public string Nomies { get; set; }
            public long CodiesEmec { get; set; }  
            public long NomiesEmec { get; set; }  
            public List<CursoProf> Cursos = new List<CursoProf>();
    } 
    
}