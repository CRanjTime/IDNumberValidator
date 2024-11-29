# ID Number Validation API

## Overview

The **ID Number  Validation API** provides an endpoint to validate identification numbers using different algorithm. It verifies whether the given Id number adheres to the algorithmic checksum standards.

---

## API Endpoint

### POST `/ValidateIdNumber`

#### Request
- **Content-Type:** `application/json`
- **Body:**
  ```json
  {
    "idNumber": "17893729974",
    "type": 1
  }
  ```

##### Request Details
- **idNumber**: Id Number to validate
- **type**: Type of Id

**(1) Credit Card**

Currently only 1 type is supported and that is Credit Card.

#### Responses
| Status Code                 | Description                                                | Example Response                                                                       |
|-----------------------------|------------------------------------------------------------|----------------------------------------------------------------------------------------|
| `200 OK`                    | Validation successful.                                     | `{ "valid": true, "type": "CreditCard" }`                                              |
| `400 Bad Request`           | Unupported Id Type. Ongoing Development.                   | `{ "error": "Validation for Social Security Number (Type 2) is under construction." }` |
| `500 Internal Server Error` | Invalid Id Type. Provided a type that was not in the list. | `{ "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1", "title": "Invalid Type. Supported types are CreditCard (1) and SocialSecurity (2).", "status": 500 }`   |

---

## Validation Rules

The input Id number must:
- Be a numeric string (no letters or special characters).
- Pass the algorithm's checksum.

---

## Future Enhancements

- New algorithm for checking validating Credit Card
- Add Support for Other Id Types

---