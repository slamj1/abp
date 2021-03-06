using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentStoreFactory _documentStoreFactory;

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDocumentStoreFactory documentStoreFactory)
        {
            _projectRepository = projectRepository;
            _documentStoreFactory = documentStoreFactory;
        }

        public virtual async Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDto(
                project,
                input.Name,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDto(
                project,
                project.DefaultDocumentName,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetNavigationAsync(GetNavigationDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDto(
                project,
                project.NavigationDocumentName,
                input.Version
            );
        }

        public async Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);
            var store = _documentStoreFactory.Create(project.DocumentStoreType);

            var documentResource = await store.GetResource(project, input.Name, input.Version);

            return ObjectMapper.Map<DocumentResource, DocumentResourceDto>(documentResource);
        }

        protected virtual async Task<DocumentWithDetailsDto> GetDocumentWithDetailsDto(
            Project project, 
            string documentName, 
            string version)
        {
            var store = _documentStoreFactory.Create(project.DocumentStoreType);
            var document = await store.GetDocument(project, documentName, version);

            return CreateDocumentWithDetailsDto(project, document);
        }

        protected virtual DocumentWithDetailsDto CreateDocumentWithDetailsDto(Project project, Document document)
        {
            var documentDto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);
            documentDto.Project = ObjectMapper.Map<Project, ProjectDto>(project);
            return documentDto;
        }
    }
}