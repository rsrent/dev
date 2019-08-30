import 'package:bms_dart/repositories.dart';
export 'package:bms_dart/src/repositories/repository_provider.dart';
import '../api_path.dart';
import 'authentication_api.dart';
import 'holiday_fake_api.dart';
import 'agreement_api.dart';
import 'contract_api.dart';
import 'absence_reason_api.dart';
import 'conversation_api.dart';
import '../conversation_firestore.dart';
import 'post_fake_api.dart';

class FakeFlutterRepositoryProvider extends RepositoryProvider {
  FakeFlutterRepositoryProvider() {
    api = Api();
    _authenticationRepository = AuthenticationRepository(AuthenticationApi());
    _agreementRepository = AgreementRepository([AgreementApi()]);
    _contractRepository = ContractRepository([ContractApi()]);
    _absenceReasonRepository = AbsenceReasonRepository([AbsenceReasonApi()]);
    _conversationRepository = ConversationRepository(
        ConversationFirestore(_authenticationRepository));

    _postRepository = PostRepository([PostFakeApi()]);

    _holidayRepository = HolidayRepository([HolidayFakeApi()]);
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

  @override
  NotiRepository notiRepository() {
    // TODO: implement notiRepository
    return null;
  }

  PostRepository _postRepository;
  @override
  PostRepository postRepository() => _postRepository;

  WorkRepository _workRepository;
  @override
  WorkRepository workRepository() => _workRepository;

  WorkContractRepository _workContractRepository;
  @override
  WorkContractRepository workContractRepository() => _workContractRepository;

  HolidayRepository _holidayRepository;
  @override
  HolidayRepository holidayRepository() => _holidayRepository;

  WorkRegistrationRepository _workRegistrationRepository;
  @override
  WorkRegistrationRepository workRegistrationRepository() =>
      _workRegistrationRepository;

  AccidentReportRepository _accidentReportRepository;
  @override
  AccidentReportRepository accidentReportRepository() =>
      _accidentReportRepository;

  @override
  LogRepository logRepository() {
    // TODO: implement logRepository
    return null;
  }

  @override
  ProjectRepository projectRepository() {
    // TODO: implement projectRepository
    return null;
  }

  @override
  TaskRepository taskRepository() {
    // TODO: implement taskRepository
    return null;
  }

  @override
  TaskCompletedRepository taskCompletedRepository() {
    // TODO: implement taskCompletedRepository
    return null;
  }

  @override
  QualityReportRepository qualityReportRepository() {
    // TODO: implement qualityReportRepository
    return null;
  }

  @override
  CommentRepository commentRepository() {
    // TODO: implement commentRepository
    return null;
  }

  @override
  AddressRepository addressRepository() {
    // TODO: implement addressRepository
    return null;
  }

  @override
  FolderRepository folderRepository() {
    // TODO: implement folderRepository
    return null;
  }

  @override
  DocumentRepository documentRepository() {
    // TODO: implement documentRepository
    return null;
  }

  @override
  ProjectRoleRepository projectRoleRepository() {
    // TODO: implement projectRoleRepository
    return null;
  }

  @override
  ClientRepository clientRepository() {
    // TODO: implement clientRepository
    return null;
  }
}
