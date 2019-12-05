namespace Censo.API.Resultados
{
    public static class ComplementoCargaHoraria
    {
        public static double CalculaGap(string _Target, double _Ds, double _Fs )
        {
            string REGIME;
            double FS;
            double DS;
                        
            REGIME = _Target;                        
            DS = _Ds;
            FS = _Fs;

            //double soma = FORA DE SALA + DENTRO DE SALA;
            //double soma = FS + DS;
           
            if (REGIME.ToUpper() == "TP")    /* Tempo parcial */
            {
                int Comple = 0;
                int CT = 12;
                double DFS = FS;

                for (int i = 1; i <= CT; i++)
                {
                    //if (DFS >= 0.25 * CT)
                    {
                        Comple++;
                        FS = FS + 1;
                      }
                    if (DS + FS >= CT && (FS >= 0.25 * (FS+DS) || FS >= 0.25 * 40))
                    {
                        break;
                    }
                }
                if (CT == FS + DS)
                    if (Comple == 0)
                    { FS = 0; }
                return Comple;
                
            }
            else  /* integral */
            {
                int Comple = 0
                    ;
                int CT = 40;
                double DFS = FS;
                //double soma1 = FS + DS;

                for (int i = 0; i <= CT; i++)
                {
                     
                        {
                            Comple++;
                            FS = FS + 1;
                            DFS = FS;
                            
                        }
                        if (DS + FS >= CT && (DFS >= 20))
                        {
                            break;
                        }
                 }
                 return Comple;

            }
        }
    }
}
