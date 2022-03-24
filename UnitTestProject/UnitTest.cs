
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using LocalNote.Models;
using LocalNote.Repositories;
using LocalNote.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Storage;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void test_CheckNoteTitle()
        {
            //Returns false if a note with this title already exists in the package folder
            string existTitle = "newtest";
            bool result = LocalNoteRepo.CheckNoteTitle(existTitle);

            Assert.AreEqual(false, result);

        }

        [TestMethod]
        public void Test_Loadfile()
        {
            //Beacuse address of the unitTestProject storage file is different from
            //that of the main program, I copied the code of LocalNoteRepo.LoadNote(); for test.

            //I store the exNote file at this address for testing
            string _basepath = @"D:\c#\LocalNote\UnitTestProject\bin\x86\Debug\AppX\noteRepo";
            string[] _strDataFiles = Directory.GetFiles(_basepath);

            List<NoteModel> exNotes = new List<NoteModel>();
            exNotes.Add(new NoteModel("newtest", "asdasdb"));

            List<NoteModel> Notes = new List<NoteModel>();

            foreach (string file in _strDataFiles)
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);
                string content = "";
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    content += line;
                }

                NoteModel note = new NoteModel(Path.GetFileName(file), content);
                Notes.Add(note);
                sr.Close();
            }
            Assert.AreEqual(exNotes.Count, Notes.Count);
        }

        [TestMethod]
        public void TestNullArg()
        {
            //Test reading the note file in the main ApplicationData.Current.LocalFolder
            bool callFailed = false;
            try
            {
                LocalNoteRepo.LoadNote();
            }
            catch (Exception)
            {
                callFailed = true;
            }
            Assert.IsTrue(callFailed, "No permission to access ApplicationData.Current.LocalFolder of main function;");
        }
        
        [TestMethod]
        public void test_getContent()
        {
            //Test the getcontent function in NoteModel,
            //this function returns the content of the corresponding title
            List<NoteModel> test= new List<NoteModel>();
            test.Add(new NoteModel("abc", "def"));
            test.Add(new NoteModel("def", "xyz"));
            //that function only use for unit test
            string result =NoteModel.getContent(test, "abc");

            Assert.AreEqual("def", result);
        }

    }  
}
