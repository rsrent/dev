
class WorkDoneSet {
  DateTime time;
  WorkDone logs;
  WorkDone reports;
  WorkDone windows;
  WorkDone fancoils;
  WorkDone periodics;

  WorkDoneSet(json) {
    time = DateTime.now();
    logs = WorkDone('Logs', json['logs'], time);
    logs = WorkDone('Kvalitetsrapporter', json['reports'], time);
    logs = WorkDone('Vinduer', json['windows'], time);
    logs = WorkDone('Fan coils', json['fancoils'], time);
    logs = WorkDone('Periodisk', json['periodics'], time);
  }
}


class WorkDone {
  DateTime time;
  String work;
  int number;
  WorkDone(this.work, this.number, this.time);
}
