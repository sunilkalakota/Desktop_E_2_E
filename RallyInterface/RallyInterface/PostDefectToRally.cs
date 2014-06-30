using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rally.RestApi;
using Rally.RestApi.Response;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System;
using RestAPI;
using System.Threading;

namespace RestAPI
{
   public class PostDefectToRally
    {
       //LogDefect Methig will hold three parameters as input
       public static void LogDefect(string[] arrayvalues)
       {
           //string strXmlFilePath = arrayvalues[0]; //XML Read Path
           //string strXMLParentNode = arrayvalues[1]; //XML Parent Node

           string strXmlFilePath = "C:\\Temp\\DefectXMLLog.xml"; //XML Read Path
           string strXMLParentNode = "DefectDetails"; //XML Parent Node

           //Getting Rally UserName and Password
            string strusername = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "UserName");
            string strpassword = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "Password");
           
           Console.WriteLine("***************Posting Defect to Rally***************");
           //var restAPI = new RallyRestApi(Rally_userName, password);
            var restAPI = new RallyRestApi(strusername, strpassword);
            
            // Log Defect into rally
            //String workspaceRef = "/workspace/19483522386d"; //SandBOX Workspace
            //String projectRef = "/project/19483522386d"; //SandBOX Project
            String workspaceRef = "/workspace/11976231838";
            String projectRef = "/project/11976231838"; 

           DynamicJsonObject toCreate = new DynamicJsonObject();
            
            
           //Getting Rally UserName and Password
           string strTestCaseID = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "TestcaseID");
           string strDescription = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "TCErrDescription");
           
           //Pass Defect Name/Title
           toCreate["Name"] = "Automation Defect - " + strTestCaseID + " " + strDescription;
            Console.WriteLine("Passing Defect Name");
            
           //Pass Defect description
            toCreate["Description"] = strTestCaseID + " " + strDescription;
            Console.WriteLine("Passing Defect Description");
           
            // Pass Release Testing tag
            ArrayList tagList = new ArrayList();
            DynamicJsonObject myTag = new DynamicJsonObject();
            myTag["_ref"] = "/tag/15417619006";//"/tag/19517910523";            
            tagList.Add(myTag);

            toCreate["Tags"] = tagList;
            Console.WriteLine("Assigning Release Testing Tag to Defect");

            // Pass Region  
            string strRegion = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "Region"); 
           QueryResult allowedValues = restAPI.GetAllowedAttributeValues("Custom", "region");
            foreach (var item in allowedValues.Results)
            {
                if (item["StringValue"] == strRegion)
                toCreate["Region"] = item["StringValue"];
            }
            Console.WriteLine("Passing Region (AU/NZ)");

            // Getting the product owner in Release team
            string strModuleName = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "TCModuleName");
            string strOwner = CommonMethods.RTProductOwner(strModuleName);
            
            QueryResult ownerValues = restAPI.GetAllowedAttributeValues("defect", "owner");
            foreach (var item in ownerValues.Results)
            {
                if (item["StringValue"] == strOwner)
                {
                    string myUserRef = item["_ref"];
                    toCreate["Owner"] = myUserRef;
                    break;
                }
            }
            Console.WriteLine("Assigning Defect Owner");

            //Assigning Priority
            QueryResult Priority = restAPI.GetAllowedAttributeValues("defect", "priority");
            foreach (var item in Priority.Results)
            {
                if (item["StringValue"] == "Low (P3)")
                    toCreate["Priority"] = item["StringValue"];

            }
            Console.WriteLine("Assigning Priority");

            //Assigning Priority
            QueryResult Severity = restAPI.GetAllowedAttributeValues("defect", "severity");
            foreach (var item in Severity.Results)
            {
                if (item["StringValue"] == "Minor Problem")
                    toCreate["Severity"] = item["StringValue"];

            }
            Console.WriteLine("Assigning Severity");

            //Assigning Kanban State
            QueryResult KanbanState = restAPI.GetAllowedAttributeValues("Defect", "c_KanbanState");
            foreach (var item in KanbanState.Results)
            {
                if (item["StringValue"] == "Backlog")
                    toCreate["c_KanbanState"] = item["StringValue"];

            }
            Console.WriteLine("Assigning Kanban State");

            //Pass Project
           toCreate["Project"] = projectRef;
                 
            //Pass State
           string strDefectState = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "DefectState");
            QueryResult stateValues = restAPI.GetAllowedAttributeValues("defect", "state");
            foreach (var item in stateValues.Results)
            {
                if (item["StringValue"] == strDefectState)
                {
                    toCreate["State"] = item["StringValue"];
                    break;
                }
            }
            Console.WriteLine("Passing Defect State");

            //Pass Environment
            string strEvironment = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "Environment");
            QueryResult environmentValues = restAPI.GetAllowedAttributeValues("defect", "environment");
            foreach (var item in environmentValues.Results)
            {
                if (item["StringValue"] == strEvironment)
                {
                    toCreate["Environment"] = item["StringValue"];
                    break;
                }                
            }
            Console.WriteLine("Passing Defect Environment");

            //Pass Submitted By
            DynamicJsonObject mySubmitUser = restAPI.GetCurrentUser();
            string mySubmitUserRef = mySubmitUser["_ref"];
            toCreate["SubmittedBy"] = mySubmitUserRef;
            Console.WriteLine("Passing Defect SubmittedBy");

            //Pass Found In build
            string strBuild = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "Build");
            toCreate["FoundInBuild"] = strBuild;
            Console.WriteLine("Passing Build Number");


            //Pass Exception Log from ClientFramework.log
            string strExpFilePath = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "AFLogPath");
            toCreate["Notes"] = CommonMethods.ExceptionLogDetails(strExpFilePath);
            Console.WriteLine("Passing Exception Log to Notes Section");

            CreateResult createResult = restAPI.Create(workspaceRef, "defect", toCreate);

            //Pass Attachment
            DynamicJsonObject defect = restAPI.GetByReference(createResult.Reference, "FormattedID");
            string defectReference = defect["_ref"];


            // Read In Image Content
            String fullImagePath = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "ScrFolderName");
            String fullImageFile = CommonMethods.Readxmlvalue(strXmlFilePath, strXMLParentNode, "ScrFileName");
            string scrFileName = fullImagePath + "\\" + fullImageFile;
            Image myImage = Image.FromFile(scrFileName);
           

            // Get length from Image.Length attribute - don't use this in REST though
            // REST expects the length of the image in number of bytes as converted to a byte array
            var imageFileLength = new FileInfo(scrFileName).Length;
            //Console.WriteLine("Image File Length from System.Drawing.Image: " + imageFileLength);

            // Convert Image to Base64 format
            string imageBase64String = CommonMethods.ImageToBase64(myImage, System.Drawing.Imaging.ImageFormat.Png);

            // Length calculated from Base64String converted back
            var imageNumberBytes = Convert.FromBase64String(imageBase64String).Length;

            // This differs from just the Length of the Base 64 String!
            //Console.WriteLine("Image File Length from Convert.FromBase64String: " + imageNumberBytes);

            // DynamicJSONObject for AttachmentContent
            DynamicJsonObject myAttachmentContent = new DynamicJsonObject();
            myAttachmentContent["Content"] = imageBase64String;

            try
            {
                CreateResult myAttachmentContentCreateResult = restAPI.Create("AttachmentContent", myAttachmentContent);
                String myAttachmentContentRef = myAttachmentContentCreateResult.Reference;
                //Console.WriteLine("Created: " + myAttachmentContentRef);

                // DynamicJSONObject for Attachment Container
                DynamicJsonObject myAttachment = new DynamicJsonObject();
                myAttachment["Artifact"] = defectReference;
                myAttachment["Content"] = myAttachmentContentRef;
                myAttachment["Name"] = fullImageFile;
                myAttachment["Description"] = "Please refer to Notes section for Additional Info.";
                myAttachment["ContentType"] = "image/png";
                myAttachment["Size"] = imageNumberBytes;
                myAttachment["User"] = mySubmitUserRef;

                CreateResult myAttachmentCreateResult = restAPI.Create("Attachment", myAttachment);

                List<string> createErrors = myAttachmentContentCreateResult.Errors;
                for (int i = 0; i < createErrors.Count; i++)
                {
                    Console.WriteLine(createErrors[i]);
                }

                String myAttachmentRef = myAttachmentCreateResult.Reference;
                Console.WriteLine("Posted Defect Successfully - Please check rally For Updates.");
                Thread.Sleep(5000);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception occurred: " + e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }
    }
}
