namespace Tasker.Api.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tasker.Api.Dtos.Tasks;
using Tasker.Api.Services.Interfaces;

[ApiController]
[Route("api/tasks")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    //Injection (Serving the Meal)
    //In TasksController.cs, you ask for the service via the constructor:
    //When a web request hits this controller, the .NET framework steps in. It sees the controller needs an ITaskService. 
    //It checks its registry, remembers your instruction to use TaskService, figures out if TaskService needs anything (it needs ITaskRepository and IMapper), builds those first, assembles the whole chain, and hands the fully built TaskService to the controller.

    [HttpGet]
    [SwaggerOperation(Summary = "Get all tasks", Description = "Retrieves a list of all tasks.")]
    [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasksAsync(CancellationToken cancellationToken)
    {
        var tasks = await taskService.GetAllTasksAsync(cancellationToken);
        return Ok(tasks);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new task", Description = "Creates a new task and returns the created task details.")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<TaskResponseDto>> CreateTaskAsync([FromBody] TaskCreateDto dto, CancellationToken cancellationToken)
    {
        var createdTask = await taskService.CreateTaskAsync(dto, cancellationToken);
        return CreatedAtAction("GetTaskById", new { taskId = createdTask.Id }, createdTask);
    }

    [HttpGet("{taskId}")]
    [SwaggerOperation(Summary = "Get task by ID", Description = "Retrieves a specific task by its unique ID.")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponseDto>> GetTaskByIdAsync([FromRoute] string taskId, CancellationToken cancellationToken)
    {
        var task = await taskService.GetTaskByIdAsync(taskId, cancellationToken);
        return Ok(task);
    }

    [HttpPut("{taskId}")]
    [SwaggerOperation(Summary = "Update a task", Description = "Updates an existing task by its ID.")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponseDto>> UpdateTaskAsync([FromRoute] string taskId, [FromBody] TaskUpdateDto dto, CancellationToken cancellationToken)
    {
        var updatedTask = await taskService.UpdateTaskAsync(taskId, dto, cancellationToken);
        return Ok(updatedTask);
    }

    [HttpDelete("{taskId}")]
    [SwaggerOperation(Summary = "Delete a task", Description = "Deletes a specific task by its ID.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute] string taskId, CancellationToken cancellationToken)
    {
        await taskService.DeleteTaskAsync(taskId, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{taskId}/assign")]
    [SwaggerOperation(Summary = "Assign a task", Description = "Assigns a specific task to a user.")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponseDto>> AssignTaskAsync([FromRoute] string taskId, [FromBody] TaskAssignDto dto, CancellationToken cancellationToken)
    {
        var assignedTask = await taskService.AssignTaskAsync(taskId, dto, cancellationToken);
        return Ok(assignedTask);
    }

    [HttpGet("{taskId}/subtasks")]
    [SwaggerOperation(Summary = "Get subtasks", Description = "Retrieves a list of subtasks for a specific task.")]
    [ProducesResponseType(typeof(IEnumerable<SubtaskResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<SubtaskResponseDto>>> GetSubtasksAsync([FromRoute] string taskId, CancellationToken cancellationToken)
    {
        var subtasks = await taskService.GetSubtasksAsync(taskId, cancellationToken);
        return Ok(subtasks);
    }

    [HttpPost("{taskId}/subtasks")]
    [SwaggerOperation(Summary = "Create a subtask", Description = "Creates a new subtask under a specific task.")]
    [ProducesResponseType(typeof(SubtaskResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubtaskResponseDto>> CreateSubtaskAsync([FromRoute] string taskId, [FromBody] SubtaskCreateDto dto, CancellationToken cancellationToken)
    {
        var createdSubtask = await taskService.CreateSubtaskAsync(taskId, dto, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, createdSubtask);
    }

    [HttpPut("{taskId}/subtasks/{id}")]
    [SwaggerOperation(Summary = "Update a subtask", Description = "Updates an existing subtask under a specific task.")]
    [ProducesResponseType(typeof(SubtaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubtaskResponseDto>> UpdateSubtaskAsync([FromRoute] string taskId, [FromRoute] string id, [FromBody] SubtaskCreateDto dto, CancellationToken cancellationToken)
    {
        var updatedSubtask = await taskService.UpdateSubtaskAsync(id, dto, cancellationToken);
        return Ok(updatedSubtask);
    }

    [HttpDelete("{taskId}/subtasks/{id}")]
    [SwaggerOperation(Summary = "Delete a subtask", Description = "Deletes an existing subtask under a specific task.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubtaskAsync([FromRoute] string taskId, [FromRoute] string id, CancellationToken cancellationToken)
    {
        await taskService.DeleteSubtaskAsync(id, cancellationToken);
        return NoContent();
    }
}
