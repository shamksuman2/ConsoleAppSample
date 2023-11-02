#region Copyright & License
/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
#endregion

using System;
using System.Text;
using Newtonsoft.Json;
using SolaceSystems.Solclient.Messaging;

/// <summary>
/// Solace Systems Messaging API tutorial: TopicPublisher
/// </summary>

namespace Tutorial
{
    /// <summary>
    /// Demonstrates how to use Solace Systems Messaging API for publishing a message
    /// </summary>
    class TopicPublisher
    {
        string VPNName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        
        const int DefaultReconnectRetries = 3;

        void Run(IContext context, string host)
        {
            // Create session properties
            SessionProperties sessionProps = new SessionProperties()
            {                
                Host = host,
                VPNName = VPNName,
                UserName = UserName,
                Password = Password,
                ReconnectRetries = DefaultReconnectRetries,
                SSLValidateCertificate = false,
                //SSLClientCertificateFile = sslFile
            };

            try
            {

            
            // Connect to the Solace messaging router
            Console.WriteLine("Connecting as {0}@{1} on {2}...", UserName, VPNName, host);
            using (ISession session = context.CreateSession(sessionProps, null, null))
            {
                ReturnCode returnCode = session.Connect();
                if (returnCode == ReturnCode.SOLCLIENT_OK)
                {
                    Console.WriteLine("Session successfully connected.");
                    PublishMessage(session);
                }
                else
                {
                    Console.WriteLine("Error connecting, return code: {0}", returnCode);
                }
            }
            }
            catch (Exception ex)
            {
                WriteToFile(JsonConvert.SerializeObject(ex));
            }
        }

        private void PublishMessage(ISession session)
        {
            // Create the message
            using (IMessage message = ContextFactory.Instance.CreateMessage())
            {
                message.Destination = ContextFactory.Instance.CreateTopic("try-me");
                // Create the message content as a binary attachment
                message.BinaryAttachment = Encoding.ASCII.GetBytes("Sample Message visual studio");

                // Publish the message to the topic on the Solace messaging router
                Console.WriteLine("Publishing message...");
                ReturnCode returnCode = session.Send(message);
                if (returnCode == ReturnCode.SOLCLIENT_OK)
                {
                    Console.WriteLine("Done.");
                }
                else
                {
                    Console.WriteLine("Publishing failed, return code: {0}", returnCode);
                }
            }
        }

        #region Main
        static void Main(string[] args)
        {
           
            string proxy = "httpc://rb-proxy-de.bosch.com:8080";
            string host = $"tcps://mr-connection-8cbir63661i.messaging.solace.cloud:55443";//%{proxy}";
            string username = "emb-aws-d-v020-dessoldev-admin";
            string vpnname = "emb-aws-d-v020-dessoldev";
            string password = "21atvu2kvfqt83okvnqhp2r53d";

            // Initialize Solace Systems Messaging API with logging to console at Warning level
            ContextFactoryProperties cfp = new ContextFactoryProperties()
            {
                SolClientLogLevel = SolLogLevel.Warning
            };
            cfp.LogToConsoleError();
            ContextFactory.Instance.Init(cfp);

            try
            {
                // Context must be created first
                using (IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null))
                {
                    // Create the application
                    TopicPublisher topicPublisher = new TopicPublisher()
                    {
                        VPNName = vpnname,
                        UserName = username,
                        Password = password
                    };

                    // Run the application within the context and against the host
                    topicPublisher.Run(context, host);
                    
                }
            }
            catch (Exception ex)
            {
                WriteToFile($"Exception thrown: {ex.Message}");
            }
            finally
            {
                // Dispose Solace Systems Messaging API
                ContextFactory.Instance.Cleanup();
            }
            Console.WriteLine("Finished.");
        }

        private static void WriteToFile(string data)
        {
            var path = @"C:\log\log.txt";
            using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(data + Environment.NewLine + "-------------------------------------------------------------" + Environment.NewLine);
            }
        }
        #endregion
    }

}