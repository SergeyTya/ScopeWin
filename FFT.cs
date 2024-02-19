using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evm_VISU
{
    using System;
    using System.Numerics;

    public class FFT
    {

        public static Complex[] CooleyTukeyFFT(Complex[] x)
        {
            int n = x.Length;
            if (n == 1)
            {
                return new Complex[] { x[0] };
            }

            Complex[] even = new Complex[n / 2];
            Complex[] odd = new Complex[n / 2];
            for (int i = 0; i < n / 2; i++)
            {
                even[i] = x[2 * i];
                odd[i] = x[2 * i + 1];
            }

            Complex[] evenTransformed = CooleyTukeyFFT(even);
            Complex[] oddTransformed = CooleyTukeyFFT(odd);

            Complex[] result = new Complex[n];
            for (int k = 0; k < n / 2; k++)
            {
                Complex polar = Complex.FromPolarCoordinates(1, -2 * Math.PI * k / n) * oddTransformed[k];
                result[k] = evenTransformed[k] + polar;
                result[k + n / 2] = evenTransformed[k] - polar;
            }

            return result;
        }

        delegate double winn_eq(int i); 

        private static Complex[] make_window(double[] signal, int win_sz, winn_eq func ) {
            int length = signal.Length;
            // int est_win_sz = (int) Math.Log(length, 2)+1;
            int est_win_sz = win_sz;
            if (est_win_sz > win_sz) est_win_sz = win_sz;
            List<Complex> windowedSignal = new List<Complex>();
            for (int i = 0; i < length; i++)
            {
                double windowValue = func(i);
                windowedSignal.Add(signal[i] * windowValue);
            }

            int tmp = est_win_sz - length;
            if (tmp > 0)
            {
                for (int i = 0; i < tmp; i++)
                {
                    windowedSignal.Add(0);
                }
            }
            return windowedSignal.ToArray();
        }


        public static Complex[] ApplyUniformWindow(double[] signal, int win_sz)
        {
            return make_window(signal, win_sz, (_) => { return 1; });
        }

        public static Complex[] ApplyFlatTopWindow(double[] signal, int win_sz)
        {
            return make_window(signal, win_sz, (i) => {
                int length = signal.Length;
                double windowValue = 0.21557895 - 0.41663158 * Math.Cos((2 * Math.PI * i) / (length - 1)) +
                                     0.277263158 * Math.Cos((4 * Math.PI * i) / (length - 1)) -
                                     0.083578947 * Math.Cos((6 * Math.PI * i) / (length - 1)) +
                                     0.006947368 * Math.Cos((8 * Math.PI * i) / (length - 1));
                return windowValue*4;
            });

        }


        public static Complex[] ApplyHammingWindow(double[] signal, int win_sz)
        {
            return make_window(signal, win_sz, (i) => {
                int length = signal.Length;
                double windowValue = 0.54 - 0.46 * Math.Cos(2 * Math.PI * i / (length - 1));
                return windowValue*2;
            });
        }

        //public static Complex[] ApplyFlatTopWindow(double[] signal)
        //{
        //    int length = signal.Length;
        //    Complex[] windowedSignal = new Complex[length];

        //    for (int i = 0; i < length; i++)
        //    {
        //        double windowValue = 0.21557895 - 0.41663158 * Math.Cos((2 * Math.PI * i) / (length - 1)) +
        //                             0.277263158 * Math.Cos((4 * Math.PI * i) / (length - 1)) -
        //                             0.083578947 * Math.Cos((6 * Math.PI * i) / (length - 1)) +
        //                             0.006947368 * Math.Cos((8 * Math.PI * i) / (length - 1));
        //        windowedSignal[i] = signal[i] * windowValue;
        //    }

        //    return windowedSignal;
        //}


        //public static Complex[] ApplyHammingWindow(double[] signal, int wind_sz) {

        //    int length = signal.Length;
        //    List<Complex> windowedSignal = new List<Complex>();
        //    for (int i = 0; i < length; i++)
        //    {
        //        double windowValue = 0.54 - 0.46 * Math.Cos(2 * Math.PI * i / (length - 1));
        //        windowedSignal.Add(signal[i] * windowValue);
        //    }

        //    int tmp = wind_sz - length;
        //    if (tmp > 0) {
        //        for (int i = 0; i < tmp; i++) {
        //            windowedSignal.Add(0);
        //        }
        //    }
        //    return windowedSignal.ToArray();
        //}

        //public static Complex[] ApplyUniformWindow(double[] signal)
        //{
        //    int length = signal.Length;
        //    Complex[] windowedSignal = new Complex[length];
        //    for (int i = 0; i < length; i++)
        //    {
        //        windowedSignal[i] = signal[i];
        //    }
        //    return windowedSignal;
        //}

        public static double [] getRealMagnitude(Complex[] spectr) {
            
            List<double> mag_arr = new List<double>();
            foreach(Complex complex in spectr)
            {
                double mag = complex.Magnitude;
                mag_arr.Add(mag);

            }
            return mag_arr.ToArray();
        }

        public static double computeTHD(double[] mag_arr) { 
            double max = mag_arr.Max();
            List<double> pow = new List<double>();
            double sum = 0;
            foreach (double mag in mag_arr) {
                double mag2 = mag * mag;
                sum += mag2;
            }
            max = max * max;
            double THD = Math.Sqrt((sum - max)/max);
            return THD;
        }

        public static Complex[] InverseCooleyTukeyFFT(Complex[] X)
        {
            int n = X.Length;
            Complex[] conjugate = new Complex[n];
            for (int i = 0; i < n; i++)
            {
                conjugate[i] = Complex.Conjugate(X[i]);
            }

            Complex[] transformedConjugate = CooleyTukeyFFT(conjugate);
            Complex[] result = new Complex[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = Complex.Conjugate(transformedConjugate[i]) / n;
            }

            return result;
        }
    }

}
