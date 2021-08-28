
// PostRequst
function postRequst(url, body, headers, method) {

    // Headers
    if (headers == null) headers = {};
    if (headers["Content-Type"] == null) headers["Content-Type"] = "application/json";
    if (headers["Accept"] == null) headers["Accept"] = "application/json";

    // Method
    if (method == null) method = "POST";

    // Post
    var task = fetch(url, {
        method: method,
        headers: headers,
        body: JSON.stringify(body)
    })

        // Response
        .then(function (response) {
            return response.text().then(function (text) {

                // Json
                try {
                    var content = JSON.parse(text);
                    if (typeof content == 'object' && content) {
                        return {
                            statusCode: response.status,
                            content: content
                        };
                    }
                } catch (e) { }

                // Null
                if (!text && text != 0) {
                    return {
                        statusCode: response.status,
                        content: "No Content"
                    };
                }

                // Text
                return {
                    statusCode: response.status,
                    content: text.replace(/^\"|\"$/g, '').replace(/\\\"/g, '"')
                };
            })
        })

        // Error
        .catch(function (error) {
            if (error instanceof TypeError == true) {
                throw {
                    statusCode: 600,
                    content: "Connection Failed"
                };
            };
            throw error;
        });

    // Return
    return task;
}