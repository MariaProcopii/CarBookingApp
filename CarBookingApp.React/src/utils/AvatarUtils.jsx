
const avatarImages = {
    MALE: "src/assets/images/profile-img-man.png",
    FEMALE: "src/assets/images/profile-img-woman.png",
    OTHER: "src/assets/images/profile-img-other.png",
    DEFAULT: "src/assets/images/profile-img-man.png"
};

export default function getAvatarSrc (gender) {
    switch (gender) {
        case "MALE":
            return avatarImages.MALE;
        case "FEMALE":
            return avatarImages.FEMALE;
        case "OTHER":
            return avatarImages.OTHER;
        default:
            return avatarImages.DEFAULT;
    }
};