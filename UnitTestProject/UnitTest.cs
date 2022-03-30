
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
        //Test connection database
        public void SqliteConncetion()
        {
            bool callSuccessful = true;
            try
            {
                LocalNoteSqlite.InitializeDB();
            }
            catch (Exception)
            {
                callSuccessful = false;
            }
            Assert.IsTrue(callSuccessful, "Successfully connected to the database");
        }

        [TestMethod]
        //Test if the file exists in the search database
        public void checkTitle()
        {
            String title = "testNewTitle";
            bool result=LocalNoteSqlite.checkTitle(title);
            //Note does not exist returns true
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        //Test if the file exists in the search database
        public void SqliteAddnote()
        {
            String title = "testTitle";
            LocalNoteSqlite.AddNote(title, "content");
            //If add note succefule return false
            bool result = LocalNoteSqlite.checkTitle(title);
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        //Test get notes from database
        public void SqliteGetNotes()
        {
            String title = "testTitle";
            List<NoteModel> notes = new List<NoteModel>();
            
            notes =LocalNoteSqlite.GetNotes();
             
            Assert.AreEqual(title, notes[0].NoteTitle);
        }



        [TestMethod]
        //Test edit the note content
        public void SqliteEdit()
        {
            
            LocalNoteSqlite.EdidNote("testTitle", "newContent");
            
            List<NoteModel> notes = new List<NoteModel>();

            notes = LocalNoteSqlite.GetNotes();
  

            Assert.AreEqual("newContent", notes[0].NoteContent);
        }

        [TestMethod]
        //Test delete the note from database
        public void SqliteDelete()
        {
            
            String title = "testDelete";
            LocalNoteSqlite.AddNote(title, "testDelete");
            LocalNoteSqlite.DeleteNote(title);

            bool result=LocalNoteSqlite.checkTitle(title);
            //If delete note succefule return true

            Assert.AreEqual(result, true);
        }


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
        public void Test_LoadNoteException()
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
            //This function returns the content of the corresponding title
            List<NoteModel> test = new List<NoteModel>();
            test.Add(new NoteModel("abc", "def"));
            test.Add(new NoteModel("def", "xyz"));
            //that function only use for unit test
            string result = NoteModel.getContent(test, "abc");

            Assert.AreEqual("def", result);
        }

        [TestMethod]
        public void test_countWords()
        {
            //Test the getcontent function in NoteModel,
            //This function returns the word count of the content of the corresponding title
            List<NoteModel> test = new List<NoteModel>();
            test.Add(new NoteModel("abc", "I'm ready for count"));
            test.Add(new NoteModel("def", "xyz"));


            int count = "I'm ready for count".Length;
            //that function only use for unit test
            int result = NoteModel.countWords(test, "abc");

            Assert.AreEqual(count, result);
        }

    }
}
