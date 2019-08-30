export 'package:bms_dart/src/repositories/repository_provider.dart';
import 'package:bms_dart/repositories.dart';

import 'accident_report_api.dart';
import 'address_api.dart';
import 'comment_api.dart';
import 'document_api.dart';
import 'folder_api.dart';
import 'log_api.dart';
import 'project_api.dart';
import 'project_role_api.dart';
import 'quality_report_api.dart';
import 'task_api.dart';
import 'task_completed_api.dart';
import 'test_resources/holiday_fake_api.dart';
import 'test_resources/post_fake_api.dart';
import 'image_controller.dart';

import 'noti_api.dart';
import 'post_api.dart';
import 'api_path.dart';
import 'holiday_api.dart';
import 'authentication_api.dart';
import 'conversation_firestore.dart';
import 'user_api.dart';
import 'agreement_api.dart';
import 'contract_api.dart';
import 'absence_api.dart';
import 'absence_reason_api.dart';
// import 'location_api.dart';
// import 'customer_api.dart';
import 'work_api.dart';
import 'work_contract_api.dart';
import 'work_registration_api.dart';
import 'client_api.dart';

class FlutterRepositoryProvider extends RepositoryProvider {
  FlutterRepositoryProvider() {
    api = Api();
    _userRepository = UserRepository([UserApi()]);
    _authenticationRepository = AuthenticationRepository(AuthenticationApi());
    _agreementRepository = AgreementRepository([AgreementApi()]);
    _contractRepository = ContractRepository([ContractApi()]);
    _absenceRepository = AbsenceRepository([AbsenceApi()]);
    _absenceReasonRepository = AbsenceReasonRepository([AbsenceReasonApi()]);
    _conversationRepository = ConversationRepository(
        ConversationFirestore(_authenticationRepository));
    _notiRepository = NotiRepository([NotiApi()]);

    _postRepository = PostRepository([PostApi()]);

    _workRepository = WorkRepository([WorkApi()]);
    _workRegistrationRepository =
        WorkRegistrationRepository([WorkRegistrationApi()]);
    _workContractRepository = WorkContractRepository([WorkContractApi()]);

    _holidayRepository = HolidayRepository([HolidayApi()]);
    _accidentReportRepository = AccidentReportRepository([AccidentReportApi()]);
    _logRepository = LogRepository([LogApi()]);
    _projectRepository = ProjectRepository([ProjectApi()]);
    _taskRepository = TaskRepository([TaskApi()]);
    _taskCompletedRepository = TaskCompletedRepository([TaskCompletedApi()]);
    _qualityReportRepository = QualityReportRepository([QualityReportApi()]);
    _commentRepository = CommentRepository([CommentApi()]);
    _addressRepository = AddressRepository([AddressApi()]);
    _folderRepository = FolderRepository([FolderApi()]);
    _documentRepository = DocumentRepository([DocumentApi()]);
    _projectRoleRepository = ProjectRoleRepository([ProjectRoleApi()]);
    _clientRepository = ClientRepository([ClientApi()]);
  }

  AuthenticationRepository _authenticationRepository;
  @override
  AuthenticationRepository authenticationRepository() =>
      _authenticationRepository;

  UserRepository _userRepository;
  @override
  UserRepository userRepository() => _userRepository;

  AgreementRepository _agreementRepository;
  @override
  AgreementRepository agreementRepository() => _agreementRepository;

  ContractRepository _contractRepository;
  @override
  ContractRepository contractRepository() => _contractRepository;

  AbsenceRepository _absenceRepository;
  @override
  AbsenceRepository absenceRepository() => _absenceRepository;

  AbsenceReasonRepository _absenceReasonRepository;
  @override
  AbsenceReasonRepository absenceReasonRepository() => _absenceReasonRepository;

  ConversationRepository _conversationRepository;
  @override
  ConversationRepository conversationRepository() => _conversationRepository;

  NotiRepository _notiRepository;
  @override
  NotiRepository notiRepository() => _notiRepository;

  PostRepository _postRepository;
  @override
  PostRepository postRepository() => _postRepository;

  WorkRepository _workRepository;
  @override
  WorkRepository workRepository() => _workRepository;

  WorkRegistrationRepository _workRegistrationRepository;
  @override
  WorkRegistrationRepository workRegistrationRepository() =>
      _workRegistrationRepository;

  WorkContractRepository _workContractRepository;
  @override
  WorkContractRepository workContractRepository() => _workContractRepository;

  HolidayRepository _holidayRepository;
  @override
  HolidayRepository holidayRepository() => _holidayRepository;

  AccidentReportRepository _accidentReportRepository;
  @override
  AccidentReportRepository accidentReportRepository() =>
      _accidentReportRepository;

  LogRepository _logRepository;
  @override
  LogRepository logRepository() => _logRepository;

  ProjectRepository _projectRepository;
  @override
  ProjectRepository projectRepository() => _projectRepository;

  TaskRepository _taskRepository;
  @override
  TaskRepository taskRepository() => _taskRepository;

  TaskCompletedRepository _taskCompletedRepository;
  @override
  TaskCompletedRepository taskCompletedRepository() => _taskCompletedRepository;

  QualityReportRepository _qualityReportRepository;
  @override
  QualityReportRepository qualityReportRepository() => _qualityReportRepository;

  CommentRepository _commentRepository;
  @override
  CommentRepository commentRepository() => _commentRepository;

  AddressRepository _addressRepository;
  @override
  AddressRepository addressRepository() => _addressRepository;

  FolderRepository _folderRepository;
  @override
  FolderRepository folderRepository() => _folderRepository;

  DocumentRepository _documentRepository;
  @override
  DocumentRepository documentRepository() => _documentRepository;

  ProjectRoleRepository _projectRoleRepository;
  @override
  ProjectRoleRepository projectRoleRepository() => _projectRoleRepository;

  ClientRepository _clientRepository;
  @override
  ClientRepository clientRepository() => _clientRepository;
}
