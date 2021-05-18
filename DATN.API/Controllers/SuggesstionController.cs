using DATN.API.Services;
using DATN.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;



namespace DATN.API.Controllers
{
    [Route("api/v1/suggesstion")]
    public class SuggesstionController : Controller
    {
        private LevenshteinService levenshteinService;

        private SkillService skillService;

        public SuggesstionController(LevenshteinService levenshteinService, SkillService skillService)
        {
            this.levenshteinService = levenshteinService;
            this.skillService = skillService;
        }


        //GET: api/v1/suggesstion/p
        [HttpGet]
        public List<string> GetSuggesstionWord([FromQuery]string p)
        {
            var result = new List<string>();

            if (skillService.GetSuggesstion_Skill(p).Count() != 0)
            {
                result = skillService.GetSuggesstion_Skill(p);
            }
            else
            {
                result = levenshteinService.calcDictDistance(p, 1);
            }


            return result;
            // return levenshteinService.calcDictDistance(p, 1);
        }


    }
}
