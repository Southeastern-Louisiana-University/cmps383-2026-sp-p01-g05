namespace Selu383.SP26.Api.Controllers
{
    public class MyController
    {
        private readonly DataContext _context;

        public MyController(DataContext context)
        {
            _context = context;
        }
    }
}

