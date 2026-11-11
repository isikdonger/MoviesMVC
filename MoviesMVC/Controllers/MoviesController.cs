#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.Services;
using APP.Models;

// Generated from Custom MVC Template.

namespace MoviesMVC.Controllers
{
    public class MoviesController : Controller
    {
        // Service injections:
        private readonly IService<MovieRequest, MovieResponse> _movieService;
        private readonly IService<DirectorRequest, DirectorResponse> _directorService;
        private readonly IService<GenreRequest, GenreResponse> _genreService;

        public MoviesController(
			IService<MovieRequest, MovieResponse> movieService
            , IService<DirectorRequest, DirectorResponse> directorService
            , IService<GenreRequest, GenreResponse> genreService
        )
        {
            _movieService = movieService;
            _directorService = directorService;
            _genreService = genreService;
        }

        private void SetViewData()
        {
            /* 
            ViewBag and ViewData are the same collection (dictionary).
            They carry extra data other than the model from a controller action to its view, or between views.
            */

            // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            ViewData["DirectorId"] = new SelectList(_directorService.List(), "Id", "FullNameR");
            ViewBag.GenreIds = new MultiSelectList(_genreService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            /*
            TempData is used to carry extra data to the redirected controller action's view.
            */

            TempData[key] = message;
        }

        // GET: Movies
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _movieService.List();
            return View(list); // return response collection as model to the Index view
        }

        // GET: Movies/Details/5
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _movieService.Single(id);
            return View(item); // return response item as model to the Details view
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(); // return Create view with no model
        }

        // POST: Movies/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(MovieRequest movie)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Insert item service logic:
                var response = _movieService.Add(movie);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(movie); // return request as model to the Create view
        }

        // GET: Movies/Edit/5
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _movieService.Edit(id);
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(item); // return request as model to the Edit view
        }

        // POST: Movies/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(MovieRequest movie)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Update item service logic:
                var response = _movieService.Update(movie);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(movie); // return request as model to the Edit view
        }

        // GET: Movies/Delete/5
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _movieService.Single(id);
            return View(item); // return response item as model to the Delete view
        }

        // POST: Movies/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var response = _movieService.Remove(id);
            SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
            return RedirectToAction(nameof(Index)); // redirect to the Index action
        }
    }
}
