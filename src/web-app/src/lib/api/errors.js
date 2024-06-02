import AppError from '../../utils/appError';

export class ApiError extends AppError {
  constructor(type, title, statusCode, detail) {
    super(detail);

    this.type = type;
    this.title = title;
    this.statusCode = statusCode;
  }

  static fromResponse(response) {
    const { type, title, status, detail } = response.data;
    return new ApiError(type, title, status, detail);
  }
}

export class ApiValidationError extends ApiError {
  constructor(type, title, statusCode, detail, errors) {
    super(type, title, statusCode, detail);

    this.errors = errors;
  }

  static fromValidationResponse(response) {
    const { type, title, status, detail, errors: rawErrors } = response.data;

    const errors = Object.entries(rawErrors).map(([type, message]) => {
      type, message;
    });

    return new ApiValidationError(type, title, status, detail, errors);
  }
}
