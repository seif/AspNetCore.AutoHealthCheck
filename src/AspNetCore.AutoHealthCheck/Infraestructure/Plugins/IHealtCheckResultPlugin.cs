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

using System.Threading.Tasks;

namespace AspNetCore.AutoHealthCheck
{
    /// <summary>
    ///     Health Check Result Plugin
    /// </summary>
    public interface IHealtCheckResultPlugin
    {
        /// <summary>
        ///     Plugin name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Do something after a result
        /// </summary>
        /// <param name="result">result</param>
        /// <returns></returns>
        Task ActionAfterResult(HealthyResponse result);

        /// <summary>
        ///     Do something after a result success
        /// </summary>
        /// <param name="result">result</param>
        /// <returns></returns>
        Task ActionAfterSuccess(HealthyResponse result);

        /// <summary>
        ///     Do something after a fail result
        /// </summary>
        /// <param name="result">result</param>
        /// <returns></returns>
        Task ActionAfterFail(HealthyResponse result);
    }
}