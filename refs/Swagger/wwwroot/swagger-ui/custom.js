if (false) {
    document.addEventListener('DOMContentLoaded', function () {
        const updateFileSizeInMetadata = (fileInput, metadataInput) => {
            const file = fileInput.files[0];
            if (file) {
                const fileSize = file.size;
                const metadata = JSON.parse(metadataInput.value || '{}');
                metadata.fileSize = fileSize;
                metadata.fileName = file.name;
                metadataInput.value = JSON.stringify(metadata);
            }
        };

        const observer = new MutationObserver(function (mutations) {
            mutations.forEach(function (mutation) {
                const fileInputs = document.querySelectorAll('input[type="file"]');
                fileInputs.forEach(fileInput => {
                    fileInput.addEventListener('change', function () {
                        const metadataInput = document.querySelector('textarea[data-property-name="metadata"]');
                        updateFileSizeInMetadata(fileInput, metadataInput);
                    });
                });
            });
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    });

}