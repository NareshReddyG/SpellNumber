using Microsoft.AspNetCore.Mvc;
using SpellNumber.Services;

namespace SpellNumber
{

    [ApiController]
    [Route("api/[controller]")]
    public class SpellNumberController : Controller
    {

        private readonly ISpellNumber _spellNumber;

        public SpellNumberController(ISpellNumber spellNumber)
        {
            _spellNumber = spellNumber;
        }

        [HttpGet]
        public string SpellNumber(double number)
        {
            return _spellNumber.SpellNumber(number);
        }
    }
}
