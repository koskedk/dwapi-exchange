# KeHMIS II DWH Data Request API
# Introduction
These APIs provides secure access to the National Data warehouse.

# Overview
To access the APIs you will require permission from **NASCOP**.

The APIs provide PHI. Protected Health Information includes any personally identifiable health information including any information about health status, provision of health care, or payment for health care  information.
Any data that can be linked, even indirectly, to a person is considered personally identifiable.

The collection, storage and processing of personal data should follow the **Data Protection Act 2019 - Kenya.**

All clients must be properly vetted and onboarding workflow and approval process followed strictly.

# Authentication
The DWAPI Identity provider implements **OpenID Connect** and **OAuth 2.0** standards

The client will request an access token from the Identity Server using its client ID and secret and then use the token to gain access to the API.

Identity Server
The Identity server provides a discovery document as a standard endpoint.  The discovery document will be used by your clients and APIs to download the necessary configuration data.

https://data.kenyahmis.org:8443/.well-known/openid-configuration


# Error Codes
501 - Unauthorized.
200 - Success.

# Rate limit
Limit to the number of requests per user will be implemented.
API implements paging of results for each data set i.e. you need to specify 
**PageSize** _(Number of records and you need)_ and **PageNumber**

# Usage

Request Access Token

```bash
curl -v https://data.kenyahmis.org:8443/connect/token \
-H "Accept: application/json" \
-H "Accept-Language: en_US" \
-u "client_id:secret" \
-d "grant_type=client_credentials"
```
Make API Request for a dataset specifying its **code** and **name**

```bash
curl -v -X GET https://data.kenyahmis.org:9783/api/Dataset?code=PDA&name=visits&pageNumber=1&pageSize=10 \
-H "Content-Type: application/json" \
-H "Authorization: Bearer <Access-Token>"
```
