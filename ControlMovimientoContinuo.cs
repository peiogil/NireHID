using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace USB_HID_con_la_PICDEM_FSUSB_18F4550
{
    class ControlMovimientoContinuo
    {
        
        public byte byteSentidoGiro;
        public bool chekEntradas;
        public float float_frecuenciaCLK;
        public UInt16 CounterTimer0;
        public const UInt16 Timer0MaxValor = 65535; //Valor 0xFFFF de los TIMER0H+TIMER0L
        public const UInt16 FclkTimer0 = 19531; //40MHZ/4=5MHZ0>5/256=19531. 256 de preescaler                            
        public const byte GiroDerechas = 0, GiroIzquierdas = 1;

        public const string patternFrPulsos = @"\b^[0-4][0-9]$|\b^[0-9]$";

        //This is the constructor
        public ControlMovimientoContinuo(bool chekEntradas)
        {
            this.chekEntradas = chekEntradas;
        }



        public bool comprobarConvertirFrecuenciaCLK(string stringFrecuenciaPulsos)
        {
            Regex rgxFrPulsos = new Regex(patternFrPulsos);
            MatchCollection matchesFrPulsos = rgxFrPulsos.Matches(stringFrecuenciaPulsos);
            if (matchesFrPulsos.Count == 0)
            {
                
                chekEntradas = false;// Operation Failed
                
            }
            else
            {
                chekEntradas = true;
               
                float_frecuenciaCLK = float.Parse(stringFrecuenciaPulsos);

                CounterTimer0 = (UInt16) (Timer0MaxValor-1 / float_frecuenciaCLK * FclkTimer0);
            }
            return chekEntradas;
        }
       
    }       
      
  
}
