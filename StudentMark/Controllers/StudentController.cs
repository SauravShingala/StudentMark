using PagedList;
using StudentMark.Context;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StudentMark.Controllers
{
    public class StudentController : Controller
    {
        db_StudentEntities dbObj = new db_StudentEntities();
        // GET: Student

        public ActionResult Student(tbl_Student obj)
        {
            return View(obj);

        }

        [HttpPost]
        public ActionResult AddStudent(tbl_Student Model)
        {
            if (ModelState.IsValid)
            {
                tbl_Student obj = new tbl_Student();
                obj.Roll_No = Model.Roll_No;
                obj.Fname = Model.Fname;
                obj.Lname = Model.Lname;
                obj.Math = Model.Math;
                obj.Phy = Model.Phy;
                obj.Chem = Model.Chem;
                obj.Total = Model.Total;
                obj.Percentage = Model.Percentage;
                
                if(Model.Roll_No == 0)
                {
                    dbObj.tbl_Student.Add(obj);
                    dbObj.SaveChanges();
                }
                else
                {
                    dbObj.Entry(obj).State = EntityState.Modified;
                    dbObj.SaveChanges();
                }
            }
            var res = dbObj.tbl_Student.ToList();
            return RedirectToAction("StudentList", res);
        }

      

        public ActionResult StudentList()
        {
            var res = dbObj.tbl_Student.ToList();
            return PartialView(res);
        }

        public ActionResult Edit(int roll_no)
        {
            var res = dbObj.tbl_Student.Where(x => x.Roll_No == roll_no).First();
            tbl_Student obj = new tbl_Student();
            obj.Roll_No = res.Roll_No;
            obj.Fname = res.Fname;
            obj.Lname = res.Lname;
            obj.Math = res.Math;
            obj.Phy = res.Phy;
            obj.Chem = res.Chem;
            obj.Total = res.Total;
            obj.Percentage = res.Percentage;


            // var List = dbObj.tbl_Student.ToList();


            return View("Student", obj);
        }

        public ActionResult View(int roll_no)
        {
            tbl_Student stu = new tbl_Student();
            stu = dbObj.tbl_Student.Find(roll_no);
            return View("View", stu);
        }

        public ActionResult Delete(int roll_no)
        {
            var res = dbObj.tbl_Student.Where(x => x.Roll_No == roll_no).First();
            dbObj.tbl_Student.Remove(res);
            dbObj.SaveChanges();

            var List = dbObj.tbl_Student.ToList();

            return View("StudentList", List);
        }
    }
}