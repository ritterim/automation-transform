using System;
using System.Diagnostics;
using Microsoft.Web.XmlTransform;

namespace RimDev.Automation.Transform
{
    /// <summary>
    /// Logs the output of the transformation task to the <see cref="System.Diagnostics.Trace"/>.
    /// </summary>
    public class TraceTransformationLogger : IXmlTransformationLogger
    {
        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void LogMessage(string message, params object[] messageArgs)
        {
            Trace.TraceInformation(message, messageArgs);
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            Trace.TraceInformation(message, messageArgs);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void LogWarning(string message, params object[] messageArgs)
        {
            Trace.TraceWarning(message, messageArgs);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void LogWarning(string file, string message, params object[] messageArgs)
        {
            Trace.TraceWarning("File: {0}, Message: {1}", file, message);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="linePosition">The line position.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            Trace.TraceWarning("File: {0}, LineNumber: {1}, LinePosition: {2}, Message: {3}", file, lineNumber, linePosition, message);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void LogError(string message, params object[] messageArgs)
        {
            Trace.TraceError(message, messageArgs);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void LogError(string file, string message, params object[] messageArgs)
        {
            Trace.TraceError("File: {0}, Message: {1}", file, message);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="linePosition">The line position.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            Trace.TraceError("File: {0}, LineNumber: {1}, LinePosition: {2}, Message: {3}", file, lineNumber, linePosition, message);
        }

        /// <summary>
        /// Logs the error from exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public void LogErrorFromException(Exception ex)
        {
            Trace.TraceError(ex.Message);
        }

        /// <summary>
        /// Logs the error from exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="file">The file.</param>
        public void LogErrorFromException(Exception ex, string file)
        {
            Trace.TraceError("File: {0}, Message: {1}", file, ex.Message);
        }

        /// <summary>
        /// Logs the error from exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="file">The file.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="linePosition">The line position.</param>
        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            Trace.TraceError("File: {0}, LineNumber: {1}, LinePosition: {2}, Message: {3}", file, lineNumber, linePosition, ex.Message);
        }

        /// <summary>
        /// Starts the section.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void StartSection(string message, params object[] messageArgs)
        {
            Trace.TraceInformation(message, messageArgs);
        }

        /// <summary>
        /// Starts the section.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {
            Trace.TraceInformation(message, messageArgs);
        }

        /// <summary>
        /// Ends the section.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void EndSection(string message, params object[] messageArgs)
        {
            Trace.TraceInformation(message, messageArgs);
        }

        /// <summary>
        /// Ends the section.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The message args.</param>
        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            Trace.TraceInformation(message, messageArgs);
        }
    }
}