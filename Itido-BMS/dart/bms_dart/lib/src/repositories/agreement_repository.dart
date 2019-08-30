import 'dart:async';

import '../models/agreement.dart';
import 'source.dart';

abstract class AgreementSource extends Source {
  Future<List<Agreement>> fetchAllAgreements();
  Future<int> createAgreement(Agreement agreement);
  Future<bool> updateAgreement(Agreement agreement);
}

class AgreementRepository extends AgreementSource {
  final List<AgreementSource> sources;

  AgreementRepository(this.sources);

  Future<List<Agreement>> fetchAllAgreements() async {
    var agreements;
    for (var source in sources) {
      agreements = await source.fetchAllAgreements();
      if (agreements != null) {
        break;
      }
    }
    return agreements;
  }

  Future<int> createAgreement(Agreement agreement) =>
      sources[0].createAgreement(agreement);

  Future<bool> updateAgreement(Agreement agreement) =>
      sources[0].updateAgreement(agreement);

  @override
  void dispose() {
    sources.forEach((source) => source?.dispose());
  }
}
