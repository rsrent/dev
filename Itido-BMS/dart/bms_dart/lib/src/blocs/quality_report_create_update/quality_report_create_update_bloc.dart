import 'dart:async';
import 'package:bloc/bloc.dart';
import '../../../models.dart';
import '../../../repositories.dart';
import '../create_update_state_phase.dart';
import './bloc.dart';

class QualityReportCreateUpdateBloc extends Bloc<QualityReportCreateUpdateEvent,
    QualityReportCreateUpdateState> {
  final AuthenticationRepository _authenticationRepository =
      repositoryProvider.authenticationRepository();
  final QualityReportRepository _qualityReportRepository =
      repositoryProvider.qualityReportRepository();

  final int projectId;

  QualityReportCreateUpdateBloc({this.projectId});

  @override
  QualityReportCreateUpdateState get initialState =>
      QualityReportCreateUpdateState.createOrCopy(null,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);

  @override
  Stream<QualityReportCreateUpdateState> mapEventToState(
    QualityReportCreateUpdateEvent event,
  ) async* {
    if (event is PrepareCreate) {
      yield QualityReportCreateUpdateState.createOrCopy(
        null,
        qualityReport: QualityReport(createdTime: DateTime.now()),
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: true,
      );
      yield QualityReportCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }
    if (event is PrepareUpdate) {
      var qualityReport =
          await _qualityReportRepository.fetch(event.qualityReport.id);
      yield QualityReportCreateUpdateState.createOrCopy(
        null,
        qualityReport: qualityReport,
        createUpdateStatePhase: CreateUpdateStatePhase.Initial,
        isCreate: false,
      );
      yield QualityReportCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.InProgress);
    }

    if (event is DateTimeCreatedChanged)
      yield QualityReportCreateUpdateState.createOrCopy(currentState,
          qualityReportChanges: (qualityReport) =>
              qualityReport.createdTime = event.dateTime);
    if (event is DateTimeCompletedChanged)
      yield QualityReportCreateUpdateState.createOrCopy(currentState,
          qualityReportChanges: (qualityReport) =>
              qualityReport.completedTime = event.dateTime);

    if (event is Commit) {
      var newState = QualityReportCreateUpdateState.createOrCopy(currentState,
          createUpdateStatePhase: CreateUpdateStatePhase.Loading);
      yield newState;

      bool result;
      if (newState.isCreate) {
        result = (await _qualityReportRepository.create(projectId,
                qualityReport: newState.qualityReport)) !=
            null;
      } else {
        result = await _qualityReportRepository.update(
            newState.qualityReport.id, newState.qualityReport);
      }
      if (result) {
        yield QualityReportCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Successful);
      } else {
        yield QualityReportCreateUpdateState.createOrCopy(currentState,
            createUpdateStatePhase: CreateUpdateStatePhase.Failed);
      }
    }
  }
}
