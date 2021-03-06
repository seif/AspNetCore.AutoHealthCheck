﻿//MIT License
//Copyright(c) 2017 David Revoledo

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
// Project Lead - David Revoledo davidrevoledo@d-genix.com

using System.Collections.Generic;

namespace AspNetCore.AutoHealthCheck
{
    /// <summary>
    ///     Probe Check Result.
    /// </summary>
    public class ProbeResult
    {
        private ProbeResult()
        {
        }

        /// <summary>
        ///     Indicate if the probe was successfully or not.
        /// </summary>
        public bool Succeed { get; protected set; }

        /// <summary>
        ///     Indicate the message if the probe was not successfully.
        /// </summary>
        public string ErrorMessage { get; protected set; }

        /// <summary>
        ///     Custom information.
        /// </summary>
        public Dictionary<string, string> CustomData { get; set; } = new Dictionary<string, string>();

        /// <summary>
        ///     Probe name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        ///     Return an instance of a probe execution that was successfully.
        /// </summary>
        /// <returns></returns>
        public static ProbeResult Ok()
        {
            return new ProbeResult
            {
                Succeed = true
            };
        }

        /// <summary>
        ///     Return an instance of a probe execution that failed.
        /// </summary>
        /// <param name="errorMessage">error message</param>
        /// <returns></returns>
        public static ProbeResult Error(string errorMessage)
        {
            return new ProbeResult
            {
                ErrorMessage = errorMessage,
                Succeed = false
            };
        }
    }
}