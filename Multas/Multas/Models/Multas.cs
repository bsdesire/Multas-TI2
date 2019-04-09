using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Multas
    {
        public int ID { get; set; }

        public string Infracao { get; set; }

        public string LocalDaMulta { get; set; }

        public decimal ValorMulta { get; set; }

        public DateTime DataDaMulta { get; set; }

        //************************************
        // Criação das chaves forasteiras
        //************************************

        // FK para Viaturas
        [ForeignKey("Viatura")]
        public int ViaturaFK { get; set; } // Base de Dados
        public Viaturas Viatura { get; set; } // C#

        // FK para Condutores
        [ForeignKey("Condutor")]
        public int CondutorFK { get; set; } // Base de Dados
        public Condutores Condutor { get; set; } // C#

        // FK para Agentes
        [ForeignKey("Agente")]
        public int AgenteFK { get; set; } // Base de Dados
        public Agentes Agente { get; set; } // C#


    }
}