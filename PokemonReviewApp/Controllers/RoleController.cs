using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RoleController> _logger;

    public RoleController(
        IRoleRepository roleRepository,
        IMapper mapper,
        ILogger<RoleController> logger)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: api/role
    [HttpGet]
    public IActionResult GetRoles()
    {
        var roles = _roleRepository.GetRoles();
        return Ok(_mapper.Map<IEnumerable<RoleDto>>(roles));
    }

    // GET: api/role/deleted
    [Authorize(Roles = "Admin")]
    [HttpGet("deleted")]
    public IActionResult GetDeletedRoles()
    {
        var deleted = _roleRepository.GetDeletedRoles();
        return Ok(_mapper.Map<IEnumerable<RoleDto>>(deleted));
    }

    // GET: api/role/{id}
    [HttpGet("{id}")]
    public IActionResult GetRole(int id)
    {
        var role = _roleRepository.GetRoleById(id);
        if (role == null)
            return NotFound();

        return Ok(_mapper.Map<RoleDto>(role));
    }

    // POST: api/role
    [HttpPost]
    public IActionResult CreateRole(RoleCreateDto dto)
    {
        if (_roleRepository.RoleExists(dto.RoleName))
            return Conflict("Role already exists");

        var role = _mapper.Map<Role>(dto);
        _roleRepository.CreateRole(role);

        return Ok(_mapper.Map<RoleDto>(role));
    }

    // PUT: api/role/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateRole(int id, RoleCreateDto dto)
    {
        var role = _roleRepository.GetRoleById(id);
        if (role == null)
            return NotFound();

        if (_roleRepository.RoleExists(dto.RoleName) &&
            role.RoleName != dto.RoleName)
            return Conflict("Duplicate role name");

        role.RoleName = dto.RoleName;

        _roleRepository.UpdateRole(role);

        return Ok("Role updated");
    }

    // DELETE (soft): api/role/{id}
  
    [HttpDelete("{id}")]
    public IActionResult SoftDeleteRole(int id)
    {
        var role = _roleRepository.GetRoleById(id);
        if (role == null)
            return NotFound();

        _roleRepository.SoftDeleteRole(role);

        return Ok("Role deleted");
    }

    // RESTORE: api/role/restore/{id}
    [Authorize]
    [HttpPost("restore/{id}")]
    public IActionResult RestoreRole(int id)
    {
        var role = _roleRepository.GetRoleIncludingDeleted(id);
        if (role == null)
            return NotFound();

        if (!role.IsDeleted)
            return BadRequest("Role is not deleted");

        _roleRepository.RestoreRole(role);

        return Ok("Role restored");
    }
}
