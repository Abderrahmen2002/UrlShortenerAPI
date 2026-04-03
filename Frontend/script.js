async function shortenUrl() {
    const inputElement = document.getElementById("url");
    const resultElement = document.getElementById("result");
    const input = inputElement.value.trim();

    resultElement.innerHTML = ""; 

    // Frontend validation
    if (!input) {
        resultElement.innerHTML = `<p style="color:red">URL is required</p>`;
        return;
    }

    // Basic URL format check
    const urlPattern = /^(https?:\/\/)[^\s$.?#].[^\s]*$/i;
    if (!urlPattern.test(input)) {
        resultElement.innerHTML = `<p style="color:red">Invalid URL format</p>`;
        return;
    }

    try {
        const response = await fetch("https://localhost:7038/shorten", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ originalUrl: input })
        });

        // Handle backend validation errors (plain text)
        if (!response.ok) {
            const errorText = await response.text();
            resultElement.innerHTML = `<p style="color:red">${errorText}</p>`;
            return;
        }

        const data = await response.json();

        resultElement.innerHTML = `
            <p>Short URL:</p>
            <a href="${data.shortUrl}" target="_blank">${data.shortUrl}</a>
            <p style="color: gray; font-size: 14px;">This link will expire after 24 hours.</p>
        `;
    } catch (err) {
        resultElement.innerHTML = `<p style="color:red">Server unavailable</p>`;
    }
}