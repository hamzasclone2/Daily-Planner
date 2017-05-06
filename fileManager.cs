using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Security.Cryptography;

public class fileManager {

    //Direction taken from www.fizixstudios.com/labs/do/view/id/unity-file-management-and-xml
	private string	path;					//holds the application path	
		
	public void initialize()
	{
		//Debug.Log("file manager initilized.");
		path = Application.dataPath + "/SaveData";
	}
	
	private bool checkDirectory(string directory)
	{
		if(Directory.Exists(path + "/" + directory))
			return true;
		else
		{
			return false;
		}
	}
	
	public void createDirectory(string directory) //creates a file directory if one does not exist
	{
		if(checkDirectory(directory) == false)
		{
			Debug.Log ("Creating directory: " + directory);
			Directory.CreateDirectory(path + "/" + directory);
		}
		else
		{
			//Debug.Log (path + "/" + directory + " directory exists.");
		}		
	}
	
	private void deleteDirectory(string directory)
	{
		if(checkDirectory(directory) == true)
		{
			Debug.Log ("Deleting directory: " + directory);
			
			if(path != "")
				Directory.Delete(path + "/" + directory, true);
			else
				Debug.Log ("Attempting to delete entire hard drive. Cancelling.");
		}
		else
		{
			Debug.Log ("Directory Error: " + directory + " file does not exist.");
		}
	}
	
	private void moveDirectory(string originalDestination, string newDestination)
	{
		if(checkDirectory(originalDestination) == true && checkDirectory(newDestination) == false)
		{
			Debug.Log ("Moving directory: " + originalDestination);
			Directory.Move(path + "/" + originalDestination, path + "/" + newDestination);
		}
		else
		{
			Debug.Log ("Directory Error: Directory move destination does not exist or folder of the same name already exists.");
		}
	}
	
	public string [] findSubDirectories(string directory)
	{
		Debug.Log ("Checking directory " + directory + " for sub directories.");
		
		string [] subdirectoryList = Directory.GetDirectories(path + "/" + directory);
		return subdirectoryList;
	}
	
	public string [] findFiles(string directory)
	{
		Debug.Log ("Checking directory " + directory + " for files");
		
		string [] fileList = Directory.GetFiles(path + "/" + directory);
		
		return fileList;
	}
	
	public bool checkFile(string filePath)
	{
		if(File.Exists(path + "/" + filePath))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public void createFile(string directory, string filename, string filetype, string fileData, bool encrypt)
	{
		if(checkDirectory(directory) == true)
		{
			if(checkFile(directory + "/" + filename + "." + filetype) == false)
			{
				if(encrypt == false)
				{
					File.WriteAllText(path + "/" + directory + "/" + filename + "." + filetype, fileData);
					//Debug.Log("creating file " + filename + "." + filetype);
				}
				else if(encrypt == true)
				{
					fileData = encryptData(fileData);
					File.WriteAllText(path + "/" + directory + "/" + filename + "." + filetype, fileData);
				}
			}
			else
				Debug.Log ("File " + filename + " already exists in " + path + "/" + directory);
		}
	}
	
	public XmlDocument useXmlFile(string directory, string filename, string filetype, bool encrypt) //returns an xml document
	{
		XmlDocument xmlDoc = new XmlDocument();
		
		if(encrypt == false)
		{
			xmlDoc.Load(path + "/" + directory + "/" + filename + "." + filetype);
			Debug.Log("Loading xml file");
			return xmlDoc;
		}
		else
		{
			//Debug.Log("Using encrypt format");
			// Read the encrypted file into filedata
			string filedata = readFile(directory, filename, filetype);
			
			// Decrypt the data
			filedata = decryptData(filedata);
			
			// Create a temporary plain text file
			createFile(directory + "/", "tmp_" + filename, filetype, filedata, false);
			
			// Read the temporary file
			xmlDoc.Load(path + "/" + directory + "/tmp_" + filename + "." + filetype);
			
			deleteFile(directory + "/" + "tmp_" + filename + "." + filetype);
			
			return xmlDoc;
		}
	}

	public string useFile(string directory, string filename, string filetype, bool encrypt) //returns a document string
	{
		string filedata = readFile(directory, filename, filetype);

		if(encrypt == false)
		{
			Debug.Log("Loading " + filename + "." + filetype);
			return filedata;
		}
		else
		{
			Debug.Log("Using encrypt format");
			
			// Decrypt the data
			filedata = decryptData(filedata);
			
			return filedata;
		}
	}

	public XmlElement createXmlElement(XmlDocument xmlObject, string elementName)
	{
		XmlElement element = xmlObject.CreateElement(elementName);
		return element;
	}
		
	public XmlElement createXmlElement(XmlDocument xmlObject, string elementName, string innerValue)
	{
		XmlElement element = xmlObject.CreateElement(elementName);
		
		//process inner value
		if(innerValue != null)
		{
			element.InnerText = innerValue;
		}
		
		return element;
	}
	
	public XmlElement createXmlElement(XmlDocument xmlObject, string elementName, string innerValue, string attributeName, string attributeValue)
	{
		XmlElement element = xmlObject.CreateElement(elementName);
		
		//process inner value
		if(innerValue != null)
		{
			element.InnerText = innerValue;
		}
		
		if(attributeName != null && attributeValue != null)
		{
			element.SetAttribute(attributeName, attributeValue);
		}
		
		return element;
	}

    public XmlElement createXmlElement(XmlDocument xmlObject, string elementName, string innerValue, string[] attributeList, string[] attributeValues)
    {
        XmlElement element = xmlObject.CreateElement(elementName);

        int i = 0;

        // Process inner value
        if(innerValue != null)
        {
            element.InnerText = innerValue;
        }

        // Process attributes
        if(attributeList != null)
        {
            foreach(string attribute in attributeList)
            {
                element.SetAttribute(attribute, attributeValues[i]);

                i++;
            }
        }

        return element;
    }
	
	public string readFile(string directory, string filename, string filetype)
	{
		//Debug.Log("Reading " + directory + "/" + filename + "." + filetype);
		
		if(checkDirectory(directory) == true)
		{
			if(checkFile(directory + "/" + filename + "." + filetype) == true)
			{
				//read the file
				string fileContents = File.ReadAllText(path + "/" + directory + "/" + filename + "." + filetype);
				
				return fileContents;
			}
			else
			{
				Debug.Log("The file " + filename + " does not exist in " + path + "/" + directory);
				
				return null;
			}
		}
		else
		{
			Debug.Log("Directory Read File Error: " + path + "/" + directory + " does not exist");
			
			return null;
		}
	}
	
	public void deleteFile(string filePath)
	{
		if(File.Exists(path + "/" + filePath))
		{
			//Debug.Log("Deleting " + filePath);
			File.Delete(path + "/" + filePath);
		}
		else
		{
			Debug.Log("Delete File Error: " + path + "/" + filePath + " does not exist");
		}
	}
	
	public void updateFile(string directory, string filename, string filetype, string fileData, bool encrypt, bool replace)
	{
		Debug.Log("Updating " + directory + "/" + filename + "." + filetype);
		
		if(checkDirectory (directory) == true)
		{
			if(checkFile(directory + "/" + filename + "." + filetype) == true)
			{
				if(replace)
				{
					if(encrypt)
					{
						fileData = encryptData(fileData);
						File.WriteAllText(path + "/" + directory + "/" + filename + "." + filetype, fileData);
					}
					else
						File.WriteAllText(path + "/" + directory + "/" + filename + "." + filetype, fileData);
				}
				
				if(!replace)
				{
					if(encrypt)
					{
						fileData = encryptData(fileData);
						File.AppendAllText(path + "/" + directory + "/" + filename + "." + filetype, fileData);
					}
					else
						File.AppendAllText(path + "/" + directory + "/" + filename + "." + filetype, fileData);
				}
			}
			else
				Debug.Log("The file " + filename + " does not exist in " + path + "/" + directory);
		}
		else
			Debug.Log("Unable to create file as the directory " + directory + " does not exist");
	}
	
	public string encryptData(string toEncrypt)
	{
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
		
		// 256-AES key
		byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
		RijndaelManaged rDel = new RijndaelManaged();
		
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		
		rDel.Padding = PaddingMode.PKCS7;
		
		ICryptoTransform cTransform = rDel.CreateEncryptor();
		byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
		
		return Convert.ToBase64String (resultArray, 0, resultArray.Length);
	}
	
	public string decryptData(string toDecrypt)
	{
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes ("12345678901234567890123456789012");
		
		// AES-256 key
		byte[] toEncryptArray = Convert.FromBase64String (toDecrypt);
		RijndaelManaged rDel = new RijndaelManaged ();
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		
		rDel.Padding = PaddingMode.PKCS7;
		
		// better lang support
		ICryptoTransform cTransform = rDel.CreateDecryptor ();
		
		byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
		
		return UTF8Encoding.UTF8.GetString (resultArray);
	}
}
