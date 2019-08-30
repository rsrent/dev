
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Data;
using System.Linq;


namespace API.Repositories
{

public class RoleRepository
    {

    private readonly BMSContext _context;

    public RoleRepository(BMSContext _context)
    {
        this._context = _context;
    }


    public bool IsAdminFromFirebaseId(string firebaseId)
    {
        var user = _context.Users.FirstOrDefault(u => u.FirebaseId == firebaseId);
        if(user.Role == "Admin"){
            return true;
        }
        
        return false;
    }


    }

}