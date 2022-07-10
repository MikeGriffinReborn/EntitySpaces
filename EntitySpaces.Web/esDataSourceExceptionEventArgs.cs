/*  New BSD License
-------------------------------------------------------------------------------
Copyright (c) 2006-2012, EntitySpaces, LLC
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the EntitySpaces, LLC nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL EntitySpaces, LLC BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
-------------------------------------------------------------------------------
*/

using System;

namespace EntitySpaces.Web
{
    /// <summary>
    /// Passed in as the argument to the esException event.
    /// </summary>
    public class esDataSourceExceptionEventArgs
    {
        public esDataSourceExceptionEventArgs(Exception ex)
        {
            this.TheException = ex;
        }

        /// <summary>
        /// Set this to true to ignore the exception and continue, this would be 
        /// a rare case that you would be able to do this.
        /// </summary>
        public bool ExceptionWasHandled;

        /// <summary>
        /// The low level exception that trigger the esException event.
        /// </summary>
        public Exception TheException;

        /// <summary>
        /// The event type can tell you from where the exception was thrown
        /// </summary>
        public esDataSourceEventType EventType;

        /// <summary>
        /// Contains all of the available information at the time of the exception 
        /// during the esSelect event.
        /// </summary>
        public esDataSourceSelectEventArgs SelectArgs;

        /// <summary>
        /// Contains all of the available information at the time of the exception 
        /// during the esInsert event.
        /// </summary>
        public esDataSourceInsertEventArgs InsertArgs;

        /// <summary>
        /// Contains all of the available information at the time of the exception 
        /// during the esUpdate event.
        /// </summary>
        public esDataSourceUpdateEventArgs UpdateArgs;

        /// <summary>
        /// Contains all of the available information at the time of the exception 
        /// during the esDelete event.
        /// </summary>
        public esDataSourceDeleteEventArgs DeleteArgs;
    }
}
