using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace HidDemoWindowsForms
{
    class ControlMovimientoContinuo
    {
        
        //public byte byteSentidoGiro;
        public bool chekEntradas;
        public float float_frecuenciaCLK;
        public UInt16 CounterTimer0;
        public const UInt16 Timer0MaxValor = 65535; //Valor 0xFFFF de los TIMER0H+TIMER0L
        public const UInt16 FclkTimer0 = 19531; //40MHZ/4=5MHZ0=>5/256=19531 Hz. 256 por el preescaler                            
        public const byte GiroDerechas = 0, GiroIzquierdas = 1;
        public byte MultiplicadorStepResolution;
        public const string patternFrPulsos = @"\b^[0-9][0-9]$|\b^[0-9]$";

        //This is the constructor
        public ControlMovimientoContinuo(bool chekEntradas)
        {
            this.chekEntradas = chekEntradas;
        }



        public bool comprobarFrecuenciaCLK(string stringFrecuenciaPulsos, out float float_frecuenciaCLK)
        {
            Regex rgxFrPulsos = new Regex(patternFrPulsos);
            MatchCollection matchesFrPulsos = rgxFrPulsos.Matches(stringFrecuenciaPulsos);
            if (matchesFrPulsos.Count == 0)
            {
                
                chekEntradas = false;// Operation Failed
                float_frecuenciaCLK = 0;
            }
            else
            {
                chekEntradas = true;
               
                float_frecuenciaCLK = float.Parse(stringFrecuenciaPulsos);

               // CounterTimer0 = (UInt16) (Timer0MaxValor-1 / float_frecuenciaCLK * FclkTimer0);
            }
            return chekEntradas;
        }
        public UInt16 convertirFrecuenciaCLK(UInt16 stepPrecision, float float_frecuenciaCLK)
        {
            switch (stepPrecision)
            {
                case 1: case 9:
                    MultiplicadorStepResolution = 2;
                    break;
                case 2: case 10:
                    MultiplicadorStepResolution = 4;
                    break;
                case 3: case 11:
                    MultiplicadorStepResolution = 8;
                    break;
                case 4: case 12:
                    MultiplicadorStepResolution = 16;
                    break;
                case 5: case 13:
                    MultiplicadorStepResolution = 32;
                    break;
                case 6: case 14:
                    MultiplicadorStepResolution = 64;
                    break;
                case 7: case 15:
                    MultiplicadorStepResolution = 128;
                    break;
                default:
                    MultiplicadorStepResolution = 1;
                    break;

            }
            CounterTimer0 = (UInt16)
                (Timer0MaxValor - (1 / (float_frecuenciaCLK*MultiplicadorStepResolution) * FclkTimer0));
            return CounterTimer0;
        }
       
    }       
      
  
}
