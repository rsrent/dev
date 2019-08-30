import 'dart:convert';
import 'dart:io';
import 'dart:async';
import 'dart:isolate';
import 'dart:typed_data';

import 'package:http/http.dart';

typedef T Converter<T>(dynamic);

class ClientController<T> {
  bool _printMethod = true;
  bool _printPath = true;
  bool _printStatusCode = true;
  bool _printBody = true;
  bool _printError = true;

  final Client _client;
  final Converter<T> converter;
  final Map<String, String> Function() getHeaders;

  ClientController({
    this.converter,
    Client client,
    this.getHeaders,
  }) : this._client = client ?? Client();

  void close() {
    _client.close();
  }

  Future<T> get(
    url, {
    Map<String, String> headers,
  }) async {
    try {
      var response = await _client.get(
        url,
        headers: headers ?? (getHeaders != null ? getHeaders() : {}),
      );
      printMethod('get');
      printPath(url);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 200 && response.body != null) {
        var body = json.decode(response.body);
        var result = converter != null ? converter(body) : body;
        return result;
      }
    } catch (e) {
      printError(e);
    }
    return null;
  }

  Future<List<T>> getMany(
    url, {
    Map<String, String> headers,
  }) async {
    try {
      var response = await _client.get(
        url,
        headers: headers ?? (getHeaders != null ? getHeaders() : {}),
      );
      printMethod('getMany');
      printPath(url);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 200 && response.body != null) {
        var body = json.decode(response.body);
        var list = List.castFrom(body);
        var result = converter != null ? list.map(converter).toList() : list;
        return result;
      }
    } catch (e) {
      printError('$url: $e');
    }
    return null;
  }

  Future<int> postId(
    url, {
    Map<String, String> headers,
    body,
    Encoding encoding,
  }) async {
    try {
      var _body = body is String ? body : json.encode(body);
      var response = await _client.post(
        url,
        headers: headers ?? (getHeaders != null ? getHeaders() : {}),
        body: _body,
        encoding: encoding,
      );

      printMethod('postId');
      printPath(url);

      print(headers);
      print(_body);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 200 && response.body != null) {
        int body = json.decode(response.body);
        return body;
      }
    } catch (e) {
      printError(e);
    }
    return null;
  }

  Future<bool> postNoContent(
    url, {
    Map<String, String> headers,
    body,
    Encoding encoding,
  }) async {
    try {
      var response = await _client.post(
        url,
        headers: headers ?? (getHeaders != null ? getHeaders() : {}),
        body: body is String ? body : json.encode(body),
        encoding: encoding,
      );
      printMethod('postNoContent');
      printPath(url);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 204) {
        return true;
      }
    } catch (e) {
      printError(e);
    }
    return false;
  }

  Future<bool> put(
    url, {
    Map<String, String> headers,
    body,
    Encoding encoding,
  }) async {
    try {
      var response = await _client.put(
        url,
        headers: headers ?? (getHeaders != null ? getHeaders() : {}),
        body: body is String ? body : json.encode(body),
        encoding: encoding,
      );
      printMethod('put');
      printMethod(body);
      printPath(url);
      printStatusCode(response);
      printBody(response);

      if (response.statusCode == 204 || response.statusCode == 200) {
        return true;
      }
    } catch (e) {
      printError(e);
    }
    return false;
  }

  void printMethod(method) {
    if (_printMethod) print('method: $method');
  }

  void printPath(url) {
    if (_printPath) print('path: $url');
  }

  void printStatusCode(Response response) {
    if (_printStatusCode) print('StatusCode: ${response.statusCode}');
  }

  void printBody(Response response) {
    if (_printBody) print('Body: ${response.body}');
  }

  void printError(e) {
    if (_printError) print('Error: $e');
  }
}
