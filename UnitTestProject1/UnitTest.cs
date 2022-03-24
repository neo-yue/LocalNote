
using System;
using System.IO;
using System.Text;
using LocalNote;
using LocalNote.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
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

        //[TestMethod]
        //public void LoadNote()
        //{
        //    // StreamReader sr = new StreamReader("newtest", Encoding.Default);

        //    Assert.ThrowsException<Exception>(() => LocalNoteRepo.DeleteNote("newtest"));
        //    //LocalNoteRepo.DeleteNote();

        //}
        //[TestMethod]
        //public void TestNullArg()
        //{
        //    bool callFailed = false;
        //    try
        //    {
        //        LocalNoteRepo.LoadNote();
        //    }
        //    catch (IOException)
        //    {
        //        callFailed = true;
        //    }
        //    Assert.IsTrue(callFailed, "Expected call to MyMethod to fail with ArgumentNullException");
        //}
    }
}
