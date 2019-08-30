import '../../repositories.dart';

abstract class RepositoryProvider {
  AuthenticationRepository authenticationRepository();
  UserRepository userRepository();
  AgreementRepository agreementRepository();
  ContractRepository contractRepository();
  AbsenceRepository absenceRepository();
  AbsenceReasonRepository absenceReasonRepository();
  ConversationRepository conversationRepository();
  NotiRepository notiRepository();
  PostRepository postRepository();
  WorkRepository workRepository();
  WorkRegistrationRepository workRegistrationRepository();
  WorkContractRepository workContractRepository();
  HolidayRepository holidayRepository();
  AccidentReportRepository accidentReportRepository();
  LogRepository logRepository();
  ProjectRepository projectRepository();
  TaskRepository taskRepository();
  TaskCompletedRepository taskCompletedRepository();
  QualityReportRepository qualityReportRepository();
  CommentRepository commentRepository();
  AddressRepository addressRepository();
  FolderRepository folderRepository();
  DocumentRepository documentRepository();
  ProjectRoleRepository projectRoleRepository();
  ClientRepository clientRepository();
}

RepositoryProvider repositoryProvider;
