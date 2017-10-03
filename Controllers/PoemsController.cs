using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IncrediblePoems.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;


namespace IncrediblePoems.Controllers
{
    public class PoemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Poems
        public ActionResult Index(string category)
        {
            if (category != null)
            {
                var poems = db.Poems.Where(t => t.Category == category).OrderBy(p => p.Title).ToList();
                return View("Index", poems);
            }
            else
            {
                var poems = db.Poems.OrderBy(t => t.Title).ToList();
                return View(poems);
            }
                     
        }

        public ActionResult Search(SearchViewModel searchVm)
        {

            if(searchVm.SearchBy.IndexOf("Title") > -1)
            {
                var poems = db.Poems.Where(t => t.Title.Contains(searchVm.Search));
                return View("Index", poems);
            }

            if(searchVm.SearchBy.IndexOf("Category") > -1)
            {
                var poems = db.Poems.Where(t => t.Category.Contains(searchVm.Search)).OrderBy(p => p.Title);
                return View("Index", poems);
            }

            return View("Index", db.Poems.ToList());
        }

        public ActionResult ShowPoem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Poem poem = db.Poems.Find(id);
            ViewBag.Title = poem.Title;
            return View(poem);
        }

        // GET: Poems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poem poem = db.Poems.Find(id);
            if (poem == null)
            {
                return HttpNotFound();
            }
            return View(poem);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Poems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,CreatedDate,Category,Author,Image,Audio")] Poem poem)
        {
            if (ModelState.IsValid)
            {
                db.Poems.Add(poem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(poem);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poem poem = db.Poems.Find(id);
            if (poem == null)
            {
                return HttpNotFound();
            }
            return View(poem);
        }

        // POST: Poems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,CreatedDate,Category,Author,Image,Audio")] Poem poem)
        {
	        Poem oldPoem = new Poem();

			using (ApplicationDbContext dbOld = new ApplicationDbContext())
	        {
				oldPoem = dbOld.Poems.Find(poem.Id);
			}

            if (ModelState.IsValid)
            {
	            if (oldPoem != null)
	            {
					if (poem.Image == null)
					{
						poem.Image = oldPoem.Image ?? "";
					}
					if (poem.Audio == null)
					{
						poem.Audio = oldPoem.Audio ?? "";
					}
				}
	            
                db.Entry(poem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(poem);
        }

        [Authorize(Roles ="admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Poem poem = db.Poems.Find(id);
            if (poem == null)
            {
                return HttpNotFound();
            }
            return View(poem);
        }

        // POST: Poems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Poem poem = db.Poems.Find(id);
            db.Poems.Remove(poem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult SaveImage(ImageUploadViewModel imageVm)
        {
            var bytes = Convert.FromBase64String(imageVm.ImageData.Substring(imageVm.ImageData.IndexOf(',') + 1));

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("images");
            Random random = new Random();
            
            CloudBlockBlob newFile = container.GetBlockBlobReference(imageVm.Name);

            //switch statement
            //if .jpg = image/jpeg
            if(imageVm.Name.IndexOf(".png") > -1)
            {
                newFile.Properties.ContentType = "image/png";
            }

            if(imageVm.Name.IndexOf(".jpg") > -1)
            {
                newFile.Properties.ContentType = "image/jpg";
            }

            newFile.UploadFromByteArray(bytes, 0, bytes.Length);

            string imageUrl = ConfigurationManager.AppSettings["CdnUrl"] + "images/" + imageVm.Name;


            return PartialView("_ImagePreview", imageUrl);
        }

		[HttpPost]
		public ActionResult SaveAudioFile(ImageUploadViewModel audioVm)
		{
			var bytes = Convert.FromBase64String(audioVm.ImageData.Substring(audioVm.ImageData.IndexOf(',') + 1));

			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			CloudBlobContainer container = blobClient.GetContainerReference("audio");
			Random random = new Random();

			CloudBlockBlob newFile = container.GetBlockBlobReference(audioVm.Name);

			//switch statement
			//if .jpg = image/jpeg
			if (audioVm.Name.IndexOf(".m4a") > -1)
			{
				newFile.Properties.ContentType = "audio/m4a";
			}

			//if (audioVm.Name.IndexOf(".jpg") > -1)
			//{
			//	newFile.Properties.ContentType = "image/jpg";
			//}

			newFile.UploadFromByteArray(bytes, 0, bytes.Length);

			string audioUrl = ConfigurationManager.AppSettings["CdnUrl"] + "audio/" + audioVm.Name;


			return PartialView("_AudioPreview", audioUrl);
		}

		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class ImageUploadViewModel
    {
        public string ImageData { get; set; }
        public string Name { get; set; }
    }

	public class AudioUploadViewModel
	{
		public string AudioData { get; set; }
		public string Name { get; set; }
	}

	public class SearchViewModel
    {
        public string SearchBy { get; set; }
        public string Search { get; set; }
    }
}
