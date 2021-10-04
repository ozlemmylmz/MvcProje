using BuisnessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using FluentValidation.Results;
using BuisnessLayer.ValidationRules;

namespace MvcProje.Controllers
{
    public class WriterPanelController : Controller
    {
        // GET: WriterPanel
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        Context c = new Context();
        [HttpGet]
        public ActionResult WriterProfile(int id=0)
        {
            string p = (string)Session["WriterMail"];
            id= c.Writers.Where(x => x.WriterMail == p).Select(y =>
            y.WriterID).FirstOrDefault();
            var writervalue = wm.GetByID(id);
            return View(writervalue);
        }
        [HttpPost]
        public ActionResult WriterProfile(Writer p)
        {
            WriterValidator writervalidator = new WriterValidator();
            ValidationResult results = writervalidator.Validate(p);
            if (results.IsValid)
            {
                wm.WriterUpdate(p);
                return RedirectToAction("AllHeading","WriterPanel");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();
        }
        public ActionResult MyHeadig(string p)
        {
           
            p = (string)Session["WriterMail"];
            var writeridinfo = c.Writers.Where(x => x.WriterMail == p).Select(y =>
            y.WriterID).FirstOrDefault();    
            var values = hm.GetListByWriter(writeridinfo);
            return View(values);
        }
        [HttpGet]
        public ActionResult NewHeading()
        {
          
            List<SelectListItem> valuecatgeory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryID.ToString()

                                                  }
                                                ).ToList();
            ViewBag.vlc = valuecatgeory;
            return View();
        }
        [HttpPost]
        public ActionResult NewHeading(Heading p)
             
        {
            string writermailinfo = (string)Session["WriterMail"];
            var writeridinfo = c.Writers.Where(x => x.WriterMail == writermailinfo).Select(y => y.WriterID).FirstOrDefault();

            p.HeadingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.WriterID = writeridinfo;
            p.HeadingStatus = true;
            hm.HeadingAdd(p);
            return RedirectToAction("MyHeadig");
        }
        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            List<SelectListItem> valueheading = (from x in cm.GetList()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.CategoryName,
                                                     Value = x.CategoryID.ToString()

                                                 }
                                                   ).ToList();
            ViewBag.vlc = valueheading;
            var HeadingValue = hm.GetByID(id);
            return View(HeadingValue);
        }
        [HttpPost]
        public ActionResult EditHeading(Heading p)
        {
            hm.HeadingUpdate(p);
            return RedirectToAction("MyHeadig");
        }
        public ActionResult DeleteHeading(int id)
        {
            var headingvalue = hm.GetByID(id);
            headingvalue.HeadingStatus = false;
            hm.HedaingDelete(headingvalue);
            return RedirectToAction("MyHeadig");
        }
        public ActionResult AllHeading(int p=1)
        {

            var headings = hm.GetList().ToPagedList(p,4);
            return View(headings);
        }
    }
}